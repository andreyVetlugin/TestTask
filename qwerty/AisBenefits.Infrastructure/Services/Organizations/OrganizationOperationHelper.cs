using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Organizations
{
    using OrganizationModelDataResult = ModelDataResult<OrganizationModelData>;

    public static class OrganizationOperationHelper
    {
        public static OperationResult DeleteOrganization(this IWriteDbContext<IBenefitsEntity> writeDbContext,
            OrganizationModelDataResult modelDataResult)
        {
            if(!modelDataResult.Ok)
                return OperationResult.BuildErrorFrom(modelDataResult);

            var data = modelDataResult.Data;

            if(data.HasUsages)
                return OperationResult.BuildInnerStateError("Указанная организация используется в системе");

            writeDbContext.Remove(data.Organization);
            
            return  OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
