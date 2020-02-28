using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.Egisso;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Linq;

namespace AisBenefits.Controllers
{
    [Route("api/egisso")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePayouts)]
    public class EgissoEjectionController : Controller
    {
        private readonly IEgissoControllerContext context;

        public EgissoEjectionController(IEgissoControllerContext context)
        {
            this.context = context;
        }

        [HttpPost, Route("fact")]
        public IActionResult EjectFacts(EgissoEjectFactsForm form)
        {
            var getFactsResult = context.GetEgissoFacts(new EgissoFactPeriodData(
                form.DestinationDate,
                form.From,
                form.To
                ));

            if (!getFactsResult.Ok)
                return ApiModelResult.Create(getFactsResult);

            var facts = getFactsResult.Data
                .Where(f => f.Valid)
                .ToList();

            var buffer = new MemoryStream();
            context.WriteEgissoFactZipXml(buffer, facts);

            context.CreateEgissoFacts(facts);
            context.CreateEgissoEjectionHistory(DateTime.Now, form.DestinationDate, form.From, form.To);

            context.SaveChanges();

            return File(buffer.ToArray(), "application/zip");
        }

        [HttpPost, Route("validation")]
        public IActionResult EjectValidation()
        {
            var getFactsResult = context.GetEgissoFacts(new EgissoFactPeriodData(
                default,
                default,
                default
                ));

            if (!getFactsResult.Ok)
                return ApiModelResult.Create(getFactsResult);

            var facts = getFactsResult.Data
                .Where(f => !f.Valid)
                .ToList();

            var buffer = new MemoryStream();
            context.WriteInvalidEgissoFactCsv(buffer, facts);

            return File(buffer.ToArray(), "text/csv;charset=Windows-1251");
        }
        [HttpGet, Route("history")]
        public IActionResult History()
        {
            return new ApiResult(
                context.GetEgissoEjectionHistories()
                .AsEnumerable()
                .OrderByDescending(h => h.Date)
                .Select(h => new
                {
                    data = h.Date,
                    start = h.From,
                    end = h.To
                })
                );
        }
    }

    public interface IEgissoControllerContext :
        IReadDbContext<IBenefitsEntity>,
        IWriteDbContext<IBenefitsEntity>,
        IEgissoEjectionContext
    {

        new void SaveChanges();
    }

    public class EgissoEjectFactsForm
    {
        public DateTime DestinationDate { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
