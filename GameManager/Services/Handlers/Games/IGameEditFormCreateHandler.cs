using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using GamesManager.Models.Games;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GamesManager.Services.Handlers.Games
{
    public interface IGameEditFormCreateHandler
    {
        OperationResult Handle(GameEditForm form, ModelStateDictionary modelState);
    }
}
