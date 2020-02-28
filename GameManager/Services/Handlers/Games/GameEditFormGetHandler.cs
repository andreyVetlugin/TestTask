using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Infrastructure;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models.Games;
using Microsoft.EntityFrameworkCore;

namespace GamesManager.Services.Handlers.Games
{
    public class GameEditFormGetHandler: GameEditFormBaseHandler
    {
        public GameEditFormGetHandler(IWriteDbContext<IDbEntity> writeDbContext, IReadDbContext<IDbEntity> readDbContext) : base(writeDbContext, readDbContext)
        {
        }

        public ModelDataResult<GameEditForm> Handle(Guid gameId)
        {
            var game = readDbContext.Get<Game>()
                .IncludeDependencies()
                .FirstOrDefault(g => g.Id == gameId);
            if (game == null)
                return ModelDataResult<GameEditForm>.BuildNotExist("Игры с таким id не сущесвует");
            var form = GameEditForm.CreateFromGame(game);
            return ModelDataResult<GameEditForm>.BuildSucces(form);
        }
    }
}
