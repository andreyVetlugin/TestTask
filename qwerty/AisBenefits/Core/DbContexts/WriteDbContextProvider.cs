using DataLayer.Infrastructure.DbContexts;
using System;

namespace AisBenefits.Core.DbContexts
{
    public class WriteDbContextProvider<TIEntity> : IWriteDbContextProvider<TIEntity>
        where TIEntity : class
    {
        private readonly Func<IWriteDbContext<TIEntity>> getContext;

        public WriteDbContextProvider(Func<IWriteDbContext<TIEntity>> getContext)
        {
            this.getContext = getContext;
        }

        public IWriteDbContext<TIEntity> GetWriteContext()
        {
            return getContext();
        }
    }
}
