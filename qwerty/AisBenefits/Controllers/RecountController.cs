using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.ActionResults;
using AisBenefits.Attributes.Logging;
using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.ExtraPays;
using AisBenefits.Infrastructure.Services.Solutions;
using AisBenefits.Models.Recounts;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecountController : Controller
    {

        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly ISolutionService solutionService;
        private readonly ICurrentUserProvider currentUserProvider;

        public RecountController(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext, ISolutionService solutionService, ICurrentUserProvider currentUserProvider)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
            this.solutionService = solutionService;
            this.currentUserProvider = currentUserProvider;
        }

        [HttpPost]
        [Route("get")]
        [GetLogging]
        public IActionResult Get(RecountForm recountForm)
        {
            

            var user = currentUserProvider.GetCurrentUser();
            var newExtraPay = readDbContext.GetExtraPayModelData(recountForm.PersonInfoRootId)
                .Then(r => readDbContext.GetEditExtraPayData(
                    ExtraPayRecalculateForm.CreateExtraPensionAndGosPension(
                        recountForm.PersonInfoRootId,
                        recountForm.ExtraPension,
                        recountForm.GosPension), ExtraPayRecalculateType.ExtraPension|ExtraPayRecalculateType.GosPension, user, r))
                .Then(r => ExtraPayOperationHelper.RecalculateWithoutSave(r));

            var sols = readDbContext.Get<Solution>()
                .ByPersonRootId(recountForm.PersonInfoRootId)
                .Where(c => c.Execution < recountForm.Date)
                .ToList();

            var newSol = new Solution
            {
                PersonInfoRootId = recountForm.PersonInfoRootId,
                Type = SolutionType.Pereraschet,
                Execution = recountForm.Date,
                TotalExtraPay = newExtraPay.Data.TotalExtraPay,
                Id = Guid.NewGuid()
            };
            sols.Add(newSol);
            sols.OrderByDescending(c => c.Execution);

            decimal firstMonthSumm = SolutionHelper.CalculateSummForMonth(sols, recountForm.Date);


            var reestrElems = readDbContext.Get<Reestr>()
                .CompletedInDateInterval(DateHelper.GetFirstDayInMonth(recountForm.Date), DateTime.Now)
                .Join(readDbContext.Get<ReestrElement>().ByPersonInfoRootId(recountForm.PersonInfoRootId),
                    rees => rees.Id,
                    reesEl => reesEl.ReestrId,
                    (rees, reesEl) => new {Date = (rees.Date), ReesElement = reesEl})
                .ToList()
                .GroupBy(c => (c.Date.Year, c.Date.Month))
                .OrderBy(c=>c.Key)
                .Select(c =>
                {

                    var summ = c.Select(d => d.ReesElement.Summ).Sum();
                    var realSumm = c.Key.Item2 == recountForm.Date.Month && c.Key.Item1 == recountForm.Date.Year
                        ? firstMonthSumm
                        : newExtraPay.Data.TotalExtraPay;
                    var differ = realSumm - summ;
                    return new
                    {
                        Date = DateHelper.FormatToMonth(new DateTime(c.Key.Item1, c.Key.Item2, 1)),
                        Summ = summ,
                        RealSumm = realSumm,
                        Diff = differ
                    };
                })
                .ToList();

            var diff = reestrElems.Sum(c => c.RealSumm) - reestrElems.Sum(c => c.Summ);

            var result = new
            {
                AggRealSumm = reestrElems.Sum(c => c.RealSumm),
                AggSumm = reestrElems.Sum(c => c.Summ),
                AggDiff = reestrElems.Sum(c => c.Diff),
                MonthElems = reestrElems,
                NewSumm = newExtraPay.Data.TotalExtraPay
            };

            return new ApiResult(result);

            
        }

        [HttpPost]
        [Route("confirm")]
        [GetLogging]
        public IActionResult Confirm(RecountForm recountForm)
        {
            if (recountForm.Summ < 0)
                return new ApiResult(400);

            var user = currentUserProvider.GetCurrentUser();
            var newExtraPay = readDbContext.GetExtraPayModelData(recountForm.PersonInfoRootId)
                .Then(r => readDbContext.GetEditExtraPayData(
                    ExtraPayRecalculateForm.CreateExtraPensionAndGosPension(
                        recountForm.PersonInfoRootId,
                        recountForm.ExtraPension,
                        recountForm.GosPension), ExtraPayRecalculateType.ExtraPension | ExtraPayRecalculateType.GosPension, user, r))
                .Then(r => ExtraPayOperationHelper.RecalculateExtraPay(writeDbContext,r));
         


            var sols = readDbContext.Get<Solution>()
                .ByPersonRootId(recountForm.PersonInfoRootId)
                .Where(c => c.Execution < recountForm.Date)
                .ToList();

            var newSol = new Solution
            {
                PersonInfoRootId = recountForm.PersonInfoRootId,
                Type = SolutionType.Pereraschet,
                Execution = recountForm.Date,
                TotalExtraPay = newExtraPay.ResultModel.TotalExtraPay,
                Id = Guid.NewGuid()
            };
            sols.Add(newSol);
            sols.OrderByDescending(c => c.Execution);

            decimal firstMonthSumm = SolutionHelper.CalculateSummForMonth(sols, recountForm.Date);


            var reestrElems = readDbContext.Get<Reestr>()
                .CompletedInDateInterval(DateHelper.GetFirstDayInMonth(recountForm.Date), DateTime.Now)
                .Join(readDbContext.Get<ReestrElement>().ByPersonInfoRootId(recountForm.PersonInfoRootId),
                    rees => rees.Id,
                    reesEl => reesEl.ReestrId,
                    (rees, reesEl) => new { Date = (rees.Date), ReesElement = reesEl })
                .ToList()
                .GroupBy(c => (c.Date.Year, c.Date.Month))
                .OrderBy(c => c.Key)
                .Select(c => new
                {
                    Date = DateHelper.FormatToMonth(new DateTime(c.Key.Item1, c.Key.Item2, 1)),
                    Summ = c.Select(d => d.ReesElement.Summ).Sum(),
                    RealSumm = ((c.Key.Month == recountForm.Date.Month) && (c.Key.Year == recountForm.Date.Year))
                          ? firstMonthSumm
                          : newExtraPay.ResultModel.TotalExtraPay
                })
                .ToList();

            var diff = reestrElems.Sum(c => c.RealSumm) - reestrElems.Sum(c => c.Summ);

            var debt = readDbContext.Get<RecountDebt>().ByPersonRootId(recountForm.PersonInfoRootId).FirstOrDefault();
            if (debt == null)
            {
                debt = new RecountDebt
                {
                    Id = Guid.NewGuid(),
                    PersonInfoRootId = recountForm.PersonInfoRootId,
                    Debt = diff,
                    MonthlyPay = recountForm.Summ - newExtraPay.ResultModel.TotalExtraPay
                };

                writeDbContext.Add(debt);
            }
            else
            {
                writeDbContext.Attach(debt);
                debt.Debt += diff;
                debt.MonthlyPay = recountForm.Summ - newExtraPay.ResultModel.TotalExtraPay;
            }

            var newSolution =
                solutionService.CountFromPensionUpdate(newExtraPay.ResultModel, DateTime.Now, recountForm.Date, "Из пересчёта долга", true);
            

            return new ApiOperationResult(OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext)));
        }

    }
}