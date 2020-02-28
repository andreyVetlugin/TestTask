using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.Services
{
    public interface ILogBuilder
    {
        
        PostInfoLog BuildPostInfoLog(PostOperationType operationName, Guid EntityRootId);
    }

    public class LogBuilder : ILogBuilder
    {

        private readonly ICurrentUserProvider currentUserProvider;
        private readonly User currentUser;

        public LogBuilder(ICurrentUserProvider currentUserProvider)
        {
            this.currentUserProvider = currentUserProvider;
            currentUser = currentUserProvider.GetCurrentUser();
        }

                      

        public PostInfoLog BuildPostInfoLog(PostOperationType operationName, Guid EntityRootId)
        {
            var infoLog = new PostInfoLog
            {
                Id = Guid.NewGuid(),
                UserId = currentUser.Id,
                Date = DateTime.Now,
                Operation = operationName,
                EntityRootId = EntityRootId
            };
            return infoLog;
        }
    }
}
