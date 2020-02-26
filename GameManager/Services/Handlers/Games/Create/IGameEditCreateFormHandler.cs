using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesManager.Infrastructure.Services;
using GamesManager.Models.Game;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GamesManager.Services.Handlers.Games.Create
{
    public interface IGameEditCreateFormHandler
    {
        OperationResult Handle(GameEditForm form, ModelStateDictionary modelState);
    }
}
