using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Infrastructure;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models.Games;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace GamesManager.Services.Handlers.Games
{
    public class GameEditFormGetHandler: GameEditFormBaseHandler
    {
        public GameEditFormGetHandler(IWriteDbContext<IDbEntity> writeDbContext, IReadDbContext<IDbEntity> readDbContext, DataValidator dataValidator) : base(writeDbContext, readDbContext, dataValidator)
        {
        }

        public ModelDataResult<GameEditForm> Handle(Guid gameId, ModelStateDictionary modelState)
        {
            var game = readDbContext.Get<Game>()
                .IncludeDependencies()
                .FirstOrDefault(g => g.Id == gameId);

            var validationStateResult = dataValidator.ValidateGame(game, modelState);
            if (!validationStateResult.Ok)
                return ModelDataResult<GameEditForm>.BuildNotExist(validationStateResult.ErrorMessage);

            var form = GameEditForm.CreateFromGame(game);

            LogOperationInfo(PostOperationType.GiveGameFromDb, game.Id);
            return ModelDataResult<GameEditForm>.BuildSucces(form);
        }
    }
}
