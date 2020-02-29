using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models.Games;

namespace GamesManager.Services.Handlers.Games
{
    public class GameEditFormGetAllHandler : GameEditFormBaseHandler
    {
        public GameEditFormGetAllHandler(IWriteDbContext<IDbEntity> writeDbContext, IReadDbContext<IDbEntity> readDbContext, DataValidator dataValidator) : base(writeDbContext, readDbContext, dataValidator)
        {
        }

        public ModelDataResult<List<GameEditForm>> Handle()
        {
            var games = GameEditForm.CreateFromExistingGames(readDbContext).ToList();

            LogOperationInfo(PostOperationType.GiveGamesFromDb, Guid.Empty);
            return ModelDataResult<List<GameEditForm>>.BuildSucces(games);
        }
    }
}
