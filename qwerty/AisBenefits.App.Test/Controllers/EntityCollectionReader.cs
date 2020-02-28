using System.Collections.Generic;
using System.Linq;
using DataLayer.Entities;

namespace AisBenefits.App.Test.Controllers
{
    static class EntityCollectionReader
    {
        public static IEnumerable<TEntity> Get<TEntity>(this IEnumerable<IBenefitsEntity> entities)
        where TEntity: class, IBenefitsEntity
        {
            return entities.Select(e => e as TEntity).Where(e => e != null);
        }
    }
}
