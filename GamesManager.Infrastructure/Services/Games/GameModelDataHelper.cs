using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace GamesManager.Infrastructure.Services.Games
{
    using UserModelDataResult = ModelDataResult<UserModelData>;
    
    public static class GameModelDataHelper
    {
        public static UserModelDataResult GetGameModelData(IReadDbContext<IDbEntity> readDbContext, Guid GameInfoId)
        {
            GameInfo gameInfo = readDbContext.Get<GameInfo>();

            return readDbContext.GetUserModelData()

        }
    }
}
