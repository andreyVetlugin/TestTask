using System;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models.Games;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;

namespace GamesManager.Services.Handlers.Games
{
    public class GameEditFormCreateHandler: GameEditFormBaseHandler,IGameEditFormCreateHandler
    {
        public GameEditFormCreateHandler(IWriteDbContext<IDbEntity> writeDbContext, IReadDbContext<IDbEntity> readDbContext, DataValidator dataValidator):base(writeDbContext,readDbContext, dataValidator)
        {
        }        
        
        public OperationResult Handle(GameEditForm form, ModelStateDictionary modelState)
        {
            var stateValidationResult = dataValidator.ValidateModelState(modelState);
            if (!stateValidationResult.Ok)
                return stateValidationResult;
            
            var game = new Game
            {
                Id = Guid.NewGuid(),               
                Title = form.Title,
                ReleaseDate = form.ReleaseDate
            };

            var result = GameEditForm.JoinDependenciesToExistingGame(game, form,readDbContext,writeDbContext);
            writeDbContext.Add(game);

            //LogOperationInfo(PostOperationType.CreateNewGameIntoDb, game.Id);
            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
