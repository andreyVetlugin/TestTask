using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GamesManager.Services.Handlers.Games.Create
{
    public class GameEditCreateFormHandler: IGameEditCreateFormHandler
    {
        private readonly IWriteDbContext<IDbEntity> writeDbContext;
        private readonly IReadDbContext<IDbEntity> readDbContext;
        public OperationResult Handle(GameEditForm form, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }
    }
}
