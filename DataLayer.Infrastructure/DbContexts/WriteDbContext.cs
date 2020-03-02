using Microsoft.EntityFrameworkCore;

namespace DataLayer.Infrastructure.DbContexts
{
    public class WriteDbContext<TIEntity> : IWriteDbContext<TIEntity>
        where TIEntity: class
    {
        private readonly DbContext dbContext;

        public WriteDbContext(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Attach<TEntity>(TEntity entity)
            where TEntity: class, TIEntity
        {
            dbContext.Set<TEntity>().Attach(entity);
        }

        public void Add<TEntity>(TEntity entity)
            where TEntity : class, TIEntity
        {
            dbContext.Set<TEntity>().Add(entity);
        }

        public void Remove<TEntity>(TEntity entity)
            where TEntity : class, TIEntity
        {
            dbContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange<TEntity>(TEntity[] entityArray)
            where TEntity : class, TIEntity
        {
            dbContext.Set<TEntity>().RemoveRange(entityArray);
        }

        public void SaveChanges()
        {
           dbContext.SaveChanges();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
