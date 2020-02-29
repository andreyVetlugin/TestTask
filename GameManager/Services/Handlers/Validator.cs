using DataLayer.Entities;
using GamesManager.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GamesManager.Services.Handlers
{
    public class DataValidator
    {
        public OperationResult ValidateGame(Game game, ModelStateDictionary modelState)
        {
            if (game == null)
            {
                modelState.AddModelError("Id", "Такого Id не существует");
                return OperationResult.BuildFormError("Такого Id не существует");
            }
            return OperationResult.BuildSuccess(UnitOfWork.None());
        }
            
        public OperationResult ValidateModelState(ModelStateDictionary state) =>
            !state.IsValid ? OperationResult.BuildFormError("Проверьте введеные данные") : OperationResult.BuildSuccess(UnitOfWork.None());
    }
}
