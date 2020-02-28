using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Remotion.Linq;

namespace DataLayer.Infrastructure
{
    public static class GameDependenciesIncluder
    {
        public static IIncludableQueryable<Game, Genre> IncludeDependencies(this IQueryable<Game> queryable)
        {
            return queryable.Include(g => g.Publisher)
                .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre);
        }
    }
}
