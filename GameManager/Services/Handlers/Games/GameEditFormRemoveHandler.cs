using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using GamesManager.Models.Games;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GamesManager.Services.Handlers.Games
{
    public class GameEditFormRemoveHandler : GameEditFormBaseHandler
    {
        public GameEditFormRemoveHandler(IWriteDbContext<IDbEntity> writeDbContext, IReadDbContext<IDbEntity> readDbContext) : base(writeDbContext, readDbContext)
        { 
        }

        public OperationResult Handle(GameEditForm form, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }
    }
}
