using AisBenefits.Infrastructure.Services;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Services
{
    public class BenefitsAppContext
    {
        public IReadDbContext<IBenefitsEntity> ReadDbContext { get; }
        public IWriteDbContext<IBenefitsEntity> WriteDbContext { get; }
        public ICurrentUserProvider CurrentUserProvider { get; }

        public BenefitsAppContext(IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext, ICurrentUserProvider currentUserProvider)
        {
            ReadDbContext = readDbContext;
            WriteDbContext = writeDbContext;
            CurrentUserProvider = currentUserProvider;
        }
    }
}
