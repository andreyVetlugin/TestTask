using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models.Game;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GamesManager.Services.Handlers.Games.Create
{
    public class GameEditCreateFormHandler: IGameEditCreateFormHandler
    {
        private readonly IWriteDbContext<> writeDbContext;
        private readonly IReadDbContext<> readDbContext;
        public OperationResult Handle(GameEditForm form, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }
    }
}
