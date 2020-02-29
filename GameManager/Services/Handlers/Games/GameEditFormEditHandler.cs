using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using DataLayer.Infrastructure;
using GamesManager.Models.Games;

namespace GamesManager.Services.Handlers.Games
{
    public class GameEditFormEditHandler:GameEditFormBaseHandler
    {
        public GameEditFormEditHandler(IReadDbContext<IDbEntity> readDbContext, IWriteDbContext<IDbEntity> writeDbContext, DataValidator dataValidator):base(writeDbContext,readDbContext,dataValidator)
        { 
            
        }

        public OperationResult Handle(GameEditForm form, ModelStateDictionary modelState)
        {
            var validatinStateResult = dataValidator.ValidateModelState(modelState);
            if (!validatinStateResult.Ok)
                return validatinStateResult;

            var game = readDbContext.Get<Game>()
                .IncludeDependencies()
                .FirstOrDefault(g => g.Id == form.Id);

            var validationGameResult = dataValidator.ValidateGame(game, modelState);
            if (!validationGameResult.Ok)
                return validationGameResult;

            writeDbContext.Attach(game);
            game.ReleaseDate = form.ReleaseDate;
            var result = GameEditForm.JoinDependenciesToExistingGame(game, form, readDbContext, writeDbContext);


            LogOperationInfo(PostOperationType.EditExistingGameFromDb,game.Id);
            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
