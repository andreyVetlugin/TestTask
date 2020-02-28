using DataLayer.Infrastructure.DbContexts;
using System;

namespace AisBenefits.Core.DbContexts
{
    public class ReadDbContextProvider<TIEntity> : IReadDbContextProvider<TIEntity>
        where TIEntity : class
    {
        private readonly Func<IReadDbContext<TIEntity>> getContext;

        public ReadDbContextProvider(Func<IReadDbContext<TIEntity>> getContext)
        {
            this.getContext = getContext;
        }

        public IReadDbContext<TIEntity> GetReadContext()
        {
            return getContext();
        }
    }
}
