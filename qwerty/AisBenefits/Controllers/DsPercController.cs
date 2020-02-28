using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.Logging;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.DsPercs;
using AisBenefits.Models;
using AisBenefits.Models.DsPerc;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AisBenefits.Controllers
{
    [Route("api/dsperc"), ApiController]
    [ApiErrorHandler, PermissionFilter(RolePermissions.AdministarateCatalog)]
    public class DsPercController : Controller
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        public DsPercController(IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
        }

        [Route("getall")]
        [HttpGet]
        [GetLogging]
        public IActionResult GetAll()
        {
            var dsPercYears = readDbContext.GetAllDsPercsModelData().Data
                .Where(p => p.DsPerc.GenderType == DsPercGenderType.General)
                .GroupBy(p => p.DsPerc.Period.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    DsPercs = g.Select(p => new
                    {
                        p.DsPerc.Id,
                        p.DsPerc.Amount,
                        AgeYears = WorkAge.GetYears(p.DsPerc.AgeDays),
                        AgeMonths = WorkAge.GetMonths(p.DsPerc.AgeDays),
                        AgeDays = WorkAge.GetDays(p.DsPerc.AgeDays),
                        p.AllowEdit
                    })
                    .OrderBy(p => p.Amount)
                })
                .OrderBy(p => p.Year);

            return new ApiResult(dsPercYears);
        }

        [Route("edityear")]
        [HttpPost]
        public IActionResult EditYear(DsPercEditYearForm form)
        {
            var modelDataResult = readDbContext.BuildDsPercsEditModelData(form);
            var operationResult = writeDbContext.EditDsPercYear(modelDataResult);

            return new ApiOperationResult(operationResult);
        }

        [HttpPost, Route("delete")]
        public IActionResult Delete(GuidIdForm form)
        {
            var modelDataResult = readDbContext.GetDsPercModelData(form.Id);
            var operationResult = writeDbContext.DeleteDsPerc(modelDataResult);

            return new ApiOperationResult(operationResult);
        }
    }
}
