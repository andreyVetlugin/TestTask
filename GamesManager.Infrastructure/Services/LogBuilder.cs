using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GamesManager.Infrastructure.Services
{
    
    public static class LogInfoBuilder
    {

        public static PostInfoLog BuildLogPostInfo(PostOperationType operationType, Guid objectId)
        {
            var infoLog = new PostInfoLog
            {
                Id = Guid.NewGuid(),                
                Date = DateTime.Now,
                Operation = operationType,
                EntityRootId = objectId
            };
            return infoLog;
        }
    }
}
