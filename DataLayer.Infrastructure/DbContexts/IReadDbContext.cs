using System;
using System.Linq;

namespace DataLayer.Infrastructure.DbContexts
{
    public interface IReadDbContext<in TIEntity> : IDisposable where TIEntity : class
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : class, TIEntity;
    }
}