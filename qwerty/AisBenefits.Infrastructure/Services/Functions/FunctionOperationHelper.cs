using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Functions
{
    using FunctionModelDataResult = ModelDataResult<FunctionModelData>;

    public static class FunctionOperationHelper
    {
        public static OperationResult DeleteFunction(this IWriteDbContext<IBenefitsEntity> writeDbContext, FunctionModelDataResult dataResult)
        {
            if (!dataResult.Ok)
                return OperationResult.BuildFormError(dataResult.ErrorMessage);

            var data = dataResult.Data;

            if (data.HasUsages)
                return OperationResult.BuildInnerStateError("Указанная должность используется в системе");

            writeDbContext.Remove(data.Function);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
