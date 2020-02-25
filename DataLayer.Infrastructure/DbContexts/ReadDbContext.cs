using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataLayer.Infrastructure.DbContexts
{
    public class ReadDbContext<TIEntity> : IReadDbContext<TIEntity>
        where TIEntity: class
    {
        private readonly DbContext dbContext;

        public ReadDbContext(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<TEntity> Get<TEntity>()
            where TEntity : class, TIEntity
        {
            return dbContext.Set<TEntity>().AsQueryable().AsNoTracking();
        }

        public void Dispose()
        { 
            dbContext.Dispose();
        }
    }
}
