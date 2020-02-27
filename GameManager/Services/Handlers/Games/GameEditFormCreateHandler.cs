using System;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GamesManager.Services.Handlers.Games
{
    public class GameEditFormCreateHandler: GameEditFormBaseHandler,IGameEditFormCreateHandler
    {
        public GameEditFormCreateHandler(IWriteDbContext<IDbEntity> writeDbContext, IReadDbContext<IDbEntity> readDbContext):base(writeDbContext,readDbContext)
        {
        }        
        
        public override OperationResult Handle(GameEditForm form, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                return OperationResult.BuildFormError("Ошибка. Проверьте формат введеных данных");
            
            var game = new Game
            {
                Id = Guid.NewGuid(),               
                Title = form.Title,
                ReleaseDate = form.ReleaseDate
            };

            var result = GameEditForm.JoinDependenciesToGameFromDb(game, form,readDbContext); // использовать как-то этот operationResult?
            writeDbContext.Add(game);
            //return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
            return OperationResult.BuildSuccess(UnitOfWork.Complex(result,UnitOfWork.WriteDbContext(writeDbContext)));
        }
    }
}
