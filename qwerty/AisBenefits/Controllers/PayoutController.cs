using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Models.Payout;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AisBenefits.Attributes.Logging;
using AisBenefits.Models;
using AisBenefits.Services.Word;

namespace AisBenefits.Controllers
{
    [Route("/api/payout")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePayouts)]
    public class PayoutController : Controller
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IExtraPayWordModelBuilder extraPayWordModelBuilder;

        public PayoutController(IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext, IExtraPayWordModelBuilder extraPayWordModelBuilder)
        {
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
            this.extraPayWordModelBuilder = extraPayWordModelBuilder;
        }

        [Route("getall")]
        [HttpPost]
        [GetLogging]
        public IActionResult GetAll(PayoutGetAllForm form)
        {
            if (!readDbContext.Get<PersonInfo>().ByRootId(form.PersonId).Any())
                return BadRequest("Указанный человек не существует");

            var notCompletedReestrIds = readDbContext.Get<Reestr>()
                .NotCompleted()
                .Select(r => r.Id)
                .AsEnumerable();

            var elements = readDbContext.Get<ReestrElement>()
                .ExcludeReestrs(notCompletedReestrIds)
                .ByPersonInfoRootId(form.PersonId)
                .ToList();

            var reestrs = readDbContext.Get<Reestr>()
                .ByReestrIds(elements.Select(e => e.ReestrId))
                .ToDictionary(r => r.Id);

            var result = elements
                .GroupBy(e => e.ReestrId)
                .Select(g => (payouts: g, reestr: reestrs[g.Key]))
                .Select(g => new
                {
                    ReestrDate = g.reestr.Date,
                    Payouts = g.payouts.Select(p => new
                    {
                        p.Id,
                        Date = g.reestr.InitDate,
                        Amount = p.Summ,
                        p.Comment
                    })
                })
                .SelectMany(r => r.Payouts);

            return new ApiResult(result);
        }

        [Route("comment")]
        [HttpPost]
        public IActionResult Comment(PayoutCommentForm form)
        {
            var payout = readDbContext.Get<ReestrElement>().ById(form.PayoutId).FirstOrDefault();

            if (payout == null)
                return BadRequest("Указанная выплата не существует");

            writeDbContext.Attach(payout);
            payout.Comment = form.Comment;
            writeDbContext.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("GetWordPrintDocument")]
        [GetLogging]
        public IActionResult GetWordPrintDocument([FromBody]PayoutPrintWordForm printWordForm)
        {
            var res = extraPayWordModelBuilder.BuildTotal(printWordForm.Id, printWordForm.From, printWordForm.To);
            return res;
        }
    }
}
