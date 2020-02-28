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
        public GameEditFormEditHandler(IReadDbContext<IDbEntity> readDbContext, IWriteDbContext<IDbEntity> writeDbContext):base(writeDbContext,readDbContext)
        { 
            
        }

        public OperationResult Handle(GameEditForm form, ModelStateDictionary modelState)
        {
            
            if (!modelState.IsValid)
                return OperationResult.BuildFormError("Ошибка. Проверьте формат введеных данных");

            var game = readDbContext.Get<Game>()
                //.IncludeDependencies()
                .FirstOrDefault(g => g.Id == form.Id);
            

            if (game == null)
            {
                modelState.AddModelError("Id", "Такого Id не существует");
                return OperationResult.BuildFormError("Такого Id не существует");
            }

            //game.GameGenres.Clear();
            var result = GameEditForm.JoinDependenciesToExistingGame(game, form, readDbContext,writeDbContext); // использовать как-то этот operationResult?
            // особенно то что пишем в разные контексты ??? можно если что в врайт контексте поставить эдитед ? 
            //return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
            return OperationResult.BuildSuccess(UnitOfWork.Complex(UnitOfWork.WriteDbContext(writeDbContext)));//ReadDbContext????
        }
    }
}
