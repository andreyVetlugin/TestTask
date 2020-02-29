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
    public class GameEditFormDeleteHandler : GameEditFormBaseHandler
    {
        public GameEditFormDeleteHandler(IWriteDbContext<IDbEntity> writeDbContext, IReadDbContext<IDbEntity> readDbContext,DataValidator dataValidator) : base(writeDbContext, readDbContext,dataValidator)
        { 
        }

        public OperationResult Handle(Guid gameId, ModelStateDictionary modelState)
        {
            var game = readDbContext.Get<Game>().FirstOrDefault(g => g.Id == gameId);
            writeDbContext.Remove(game);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
