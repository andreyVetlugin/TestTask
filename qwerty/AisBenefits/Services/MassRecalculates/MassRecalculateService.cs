using System;
using System.Collections.Generic;
using System.Linq;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.ExtraPays;
using AisBenefits.Infrastructure.Services.PersonInfos;
using AisBenefits.Infrastructure.Services.PostInfoLogs;
using AisBenefits.Infrastructure.Services.Solutions;
using AisBenefits.Models.Solutions;
using DataLayer.Entities;

namespace AisBenefits.Services.MassRecalculates
{
    public static class MassRecalculateService
    {
        public static IOperationResult MassRecalculate(this BenefitsAppContext context, MassRecalculateRecalculateForm form)
        {
            var readDbContext = context.ReadDbContext;
            var writeDbContext = context.WriteDbContext;
            var currentUserProvider = context.CurrentUserProvider;

            var activePersonInfoIds = readDbContext.GetMassRecalculateablePersonInfoRootIds();
            var personInfoIds = form.IsAll ?
                activePersonInfoIds.ToList() :
                (form.IsPension
                ? readDbContext.Get<PersonInfo>()
                    .ApprovedByPayoutTypeIds(form.Ids)
                    .Select(p => p.RootId)
                    .Distinct()
                    .ToList()
                : form.VariantIds?.Count > 0
                ? readDbContext.Get<ExtraPay>()
                    .Actual()
                    .ByVariantIds(form.VariantIds)
                    .Select(p => p.PersonRootId)
                    .Distinct()
                    .ToList()
                : readDbContext.Get<WorkInfo>()
                    .ActualByFunctionIds(form.Ids)
                    .Select(w => w.PersonInfoRootId)
                    .Distinct()
                    .ToList())
                .Where(p => activePersonInfoIds.Contains(p))
                .ToList();

            var extraPaysResult = readDbContext.GetExtraPaysModelData(personInfoIds);
            if (!extraPaysResult.Ok)
                return OperationResult.BuildErrorFrom(extraPaysResult);

            var extraPays = extraPaysResult.Data;

            var user = currentUserProvider.GetCurrentUser();

            var extraPaysEditResults = (form.IsPension
                ? extraPays
                    .Select(p =>
                        readDbContext.GetEditExtraPayData(
                            ExtraPayRecalculateForm.CreateGosPension(p.Instance.PersonRootId,
                                p.Instance.GosPension * form.Koef),
                            ExtraPayRecalculateType.GosPension,
                            user,
                            p
                        ))
                : extraPays
                    .Select(p =>
                        readDbContext.GetEditExtraPayData(
                            ExtraPayRecalculateForm.CreateSalary(p.Instance.PersonRootId,
                                p.Instance.Salary * form.Koef),
                            ExtraPayRecalculateType.Salary,
                            user,
                            p
                        )))
                    .ToList();

            var recalculateResult = extraPaysEditResults
                .Select(r => writeDbContext.RecalculateExtraPay(r))
                .ToList();

            if (recalculateResult.Any(r => !r.Ok))
                return recalculateResult.First(r => !r.Ok);

            var solutionResult = readDbContext
                .GetSolutionsModelData(extraPaysEditResults.Select(r => r.Data.NewExtraPay).ToList()).Data
                .Select(d => readDbContext.BuildSolutionEditModelData(
                    new SolutionForm
                    {
                        PersonInfoRootId = d.ExtraPay.PersonRootId,
                        Execution = form.Execution,
                        Destination = form.Destination,
                        Comment = null,
                    },
                    SolutionType.Pereraschet,
                    d
                ))
                .Select(r => writeDbContext.CreateSolution(r))
                .ToList();
            if (solutionResult.Any(r => !r.Ok))
                return solutionResult.First(r => !r.Ok);

            foreach (var result in extraPaysEditResults)
            {
                writeDbContext
                    .CreatePostInfoLog(PostOperationType.ExtraPayMassRecalculate, result.Data.NewExtraPay.PersonRootId, user.Id);
            }

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }

    public class MassRecalculateRecalculateForm
    {
        public bool IsAll { get; set; }
        public bool IsPension { get; set; }
        public IReadOnlyList<Guid> Ids { get; set; }
        public IReadOnlyList<Guid> VariantIds { get; set; }
        public decimal Koef { get; set; }
        public DateTime Destination { get; set; }
        public DateTime Execution { get; set; }
    }
}
