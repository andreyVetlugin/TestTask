using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AisBenefits.Infrastructure.Services.Functions
{
    using FunctionModelDataResult = ModelDataResult<FunctionModelData>;

    public static class FunctionModelDataHelper
    {
        public static FunctionModelDataResult GetFunctionModelData(this IReadDbContext<IBenefitsEntity> readDbContext, Guid functionId)
        {
            var function = readDbContext.Get<Function>()
                .ById(functionId)
                .FirstOrDefault();

            if (function == null)
                return FunctionModelDataResult.BuildNotExist("Указанная должность не существует");

            var hasUsages = readDbContext.Get<WorkInfo>()
                .ByFunctionId(functionId)
                .Any();

            return FunctionModelDataResult.BuildSucces(new FunctionModelData(function, hasUsages));
        }

        public static ModelDataResult<List<FunctionModelData>> GetAllFunctionsModelData(this IReadDbContext<IBenefitsEntity> readDbContext)
        {
            var functions = readDbContext.Get<Function>().ToList();

            var hasUsagesSet = readDbContext.Get<WorkInfo>()
                .GroupBy(w => w.FunctionId)
                .Select(g => g.Key)
                .ToHashSet();

            return ModelDataResult<List<FunctionModelData>>.BuildSucces(
                functions
                .Select(f => new FunctionModelData(f, hasUsagesSet.Contains(f.Id)))
                .ToList()
                );
        }
    }

    public class FunctionModelData
    {
        public Function Function { get; }
        public bool HasUsages { get; }

        public FunctionModelData(Function function, bool hasUsages)
        {
            Function = function;
            HasUsages = hasUsages;
        }
    }
}
