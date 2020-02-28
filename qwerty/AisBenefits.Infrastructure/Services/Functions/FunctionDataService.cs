using System;
using System.Linq;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Functions
{
    public static class FunctionDataService
    {
        public static bool EnsureFunctionExist(this IReadDbContext<IBenefitsEntity> read,
            IWriteDbContext<IBenefitsEntity> write, string name, out Function function)
        {
            var functionName = name.Trim();

            function = read.Get<Function>()
                .ByName(functionName)
                .FirstOrDefault();

            var newFunction = function == null &&
                                  (function = new Function
                                  {
                                      Id = Guid.NewGuid(),
                                      Name = functionName
                                  }) != null;
            if (newFunction)
                write.Add(function);

            return newFunction;
        }
    }
}
