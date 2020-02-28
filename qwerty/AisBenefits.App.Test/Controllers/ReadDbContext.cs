using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.App.Test.Controllers
{
    public class ReadDbContext : IReadDbContext<IBenefitsEntity>
    {
        private readonly IEnumerable<IBenefitsEntity> entities;

        public ReadDbContext(IEnumerable<IBenefitsEntity> entities)
        {
            this.entities = entities;
        }

        public void Dispose()
        {
        }

        public IQueryable<TEntity> Get<TEntity>()
        where TEntity: class, IBenefitsEntity
        {
            return entities.Select(e => e as TEntity).Where(e => e != null)
                .Select(e => e.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(e, null) as TEntity)
                .AsQueryable();
        }
    }
}
