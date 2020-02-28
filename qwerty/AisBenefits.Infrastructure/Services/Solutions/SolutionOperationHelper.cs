using System;
using AisBenefits.Infrastructure.DTOs;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Solutions
{
    public static class SolutionOperationHelper
    {
        public static ModelDataResult<SolutionEditModelData> BuildSolutionEditPositiveModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, ISolutionForm form, ModelDataResult<SolutionModelData> solutionResult)
        {
            if(!solutionResult.Ok)
                return ModelDataResult<SolutionEditModelData>.BuildErrorFrom(solutionResult);

            var data = solutionResult.Data;

            return readDbContext.BuildSolutionEditPositiveModelData(form, data);
        }

        public static ModelDataResult<SolutionEditModelData> BuildSolutionEditPositiveModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, ISolutionForm form, SolutionModelData solution)
        {
            var type = solution.Defined && (solution.Instance.Type == SolutionType.Opredelit ||
                                            solution.Instance.Type == SolutionType.Pereraschet ||
                                            solution.Instance.Type == SolutionType.Resume)
                ? SolutionType.Pereraschet
                : solution.Defined && solution.Instance.Type == SolutionType.Pause
                    ? SolutionType.Resume
                    : !solution.Defined
                        ? SolutionType.Opredelit
                        : throw new InvalidOperationException();
            return readDbContext.BuildSolutionEditModelData(form, type, solution);
        }
            public static ModelDataResult<SolutionEditModelData> BuildSolutionEditModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, ISolutionForm form, SolutionType type, SolutionModelData solution)
        {
            if(solution.Defined && solution.Instance.PersonInfoRootId != form.PersonInfoRootId)
                throw new InvalidOperationException();

            var newSolution = new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = solution.ExtraPay.PersonRootId,
                Destination = form.Destination,
                Execution = form.Execution,
                Comment = form.Comment,

                Type = type,

                CreateTime = DateTime.Now,
                OutdateTime = null,

                DS = solution.ExtraPay.TotalPensionAndExtraPay,
                DSperc = solution.ExtraPay.DsPerc,
                TotalPension = solution.ExtraPay.TotalPension,
                TotalExtraPay = solution.ExtraPay.TotalExtraPay
            };

            var data = new SolutionEditModelData(solution, newSolution);

            return ModelDataResult<SolutionEditModelData>.BuildSucces(data);
        }

        public static OperationResult CreateSolution(this IWriteDbContext<IBenefitsEntity> writeDbContext, ModelDataResult<SolutionEditModelData> solutionEditResult)
        {
            if(!solutionEditResult.Ok)
                return OperationResult.BuildErrorFrom(solutionEditResult);

            return writeDbContext.CreateSolution(solutionEditResult.Data);
        }
        public static OperationResult CreateSolution(this IWriteDbContext<IBenefitsEntity> writeDbContext, SolutionEditModelData solution)
        {
            if (solution.Current.Defined)
            {
                writeDbContext.Attach(solution.Current.Instance);
                solution.Current.Instance.OutdateTime = solution.New.CreateTime;
            }

            writeDbContext.Add(solution.New);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }

    public class SolutionEditModelData
    {
        public SolutionModelData Current { get; }
        public Solution New { get; }


        public SolutionEditModelData(SolutionModelData current, Solution @new)
        {
            Current = current;
            New = @new;
        }
    }
}
