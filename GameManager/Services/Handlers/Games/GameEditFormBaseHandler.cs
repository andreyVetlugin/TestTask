using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesManager.Models.Games;

namespace GamesManager.Services.Handlers.Games
{
    public abstract class GameEditFormBaseHandler
    {
        protected IReadDbContext<IDbEntity> readDbContext;
        protected IWriteDbContext<IDbEntity> writeDbContext;

        public GameEditFormBaseHandler(IWriteDbContext<IDbEntity> writeDbContext, IReadDbContext<IDbEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext; 
        }
    }
}
