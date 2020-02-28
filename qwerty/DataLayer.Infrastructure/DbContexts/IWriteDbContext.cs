using System;

namespace DataLayer.Infrastructure.DbContexts
{
    public interface IWriteDbContext<in TIEntity> : IDisposable where TIEntity : class
    {
        void Add<TEntity>(TEntity entity) where TEntity : class, TIEntity;
        void Attach<TEntity>(TEntity entity) where TEntity : class, TIEntity;
        void Remove<TEntity>(TEntity entity) where TEntity : class, TIEntity;
        void RemoveRange<TEntity>(TEntity[] entityArray) where TEntity : class, TIEntity;
        void SaveChanges();
    }
}