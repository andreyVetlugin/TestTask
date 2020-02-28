using DataLayer.Entities;
using System;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.PostInfoLogs
{
    public static class PostInfoLogHelper
    {
        public static OperationResult CreatePostInfoLog(this IWriteDbContext<IBenefitsEntity> writeDbContext,
            PostOperationType operationName, Guid personInfoRootId, Guid userId)
        {
            writeDbContext.Add(Create(operationName, personInfoRootId, userId));

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        public static PostInfoLog Create(PostOperationType operationName, Guid personInfoRootId, Guid userId)
        {
            return new PostInfoLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = DateTime.Now,
                Operation = operationName,
                EntityRootId = personInfoRootId
            };
        }
    }
}
