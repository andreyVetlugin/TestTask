using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesManager.Models.Games;

namespace GamesManager.Services.Handlers.Games
{
    public abstract class GameEditFormBaseHandler
    {
        protected IReadDbContext<IDbEntity> readDbContext;
        protected IWriteDbContext<IDbEntity> writeDbContext;
        protected DataValidator dataValidator;
        protected void LogOperationInfo(PostOperationType operationType, Guid objectId)
        {
            writeDbContext.Add<IDbEntity>(LogInfoBuilder.BuildLogPostInfo(operationType,objectId));
        }

        public GameEditFormBaseHandler(IWriteDbContext<IDbEntity> writeDbContext, IReadDbContext<IDbEntity> readDbContext, DataValidator dataValidator)
        {
            this.dataValidator = dataValidator;
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext; 
        }
    }
}
