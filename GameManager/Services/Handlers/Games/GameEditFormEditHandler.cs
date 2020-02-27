using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace GamesManager.Services.Handlers.Games
{
    class GameEditFormEditHandler:GameEditFormBaseHandler
    {
        private IReadDbContext<IDbEntity> readDbContext; //все хендлеры хранят то же самое, имеют тот же конструтор и 
        private IWriteDbContext<IDbEntity> writeDbCOntext;  // 

        public GameEditFormEditHandler(IReadDbContext<IDbEntity> readDBContext, IWriteDbContext<IDbEntity> writeDbContext):base(writeDbContext,readDBContext)
        { 
            
        }

        public override OperationResult Handle(GameEditForm form, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                return OperationResult.BuildFormError("Ошибка. Проверьте формат введеных данных");

            var game = readDbContext.Get<Game>()
                .FirstOrDefault(g => g.Id == form.Id);

            if(game == null)
            {
                modelState.AddModelError("Id", "Такого Id не существует");
                return OperationResult.BuildFormError("Ошибка. Проверьте формат введеных данных");
            }

            var result = GameEditForm.JoinDependenciesToGameFromDb(game, form, readDbContext); // использовать как-то этот operationResult?
            
            //return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
            return OperationResult.BuildSuccess(UnitOfWork.Complex(result, UnitOfWork.WriteDbContext(writeDbContext)));//ReadDbContext????
        }
    }
}
