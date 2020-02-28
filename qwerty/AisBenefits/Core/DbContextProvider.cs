using DataLayer;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AisBenefits.Core
{
    public static class DbContextProvider
    {
        public static IWriteDbContext<IBenefitsEntity> GetBenefitsWriteDbContext(this IConfiguration configuration)
        {
            return new WriteDbContext<IBenefitsEntity>(
                new BenefitsContext(new DbContextOptionsBuilder()
                    .UseNpgsql(configuration.GetConnectionString("BenefitsContext")).Options)
            );
        }

        public static IReadDbContext<IBenefitsEntity> GetBenefitsReadDbContext(this IConfiguration configuration)
        {
            return new ReadDbContext<IBenefitsEntity>(
                new BenefitsContext(new DbContextOptionsBuilder()
                    .UseNpgsql(configuration.GetConnectionString("BenefitsContext")).Options)
            );
        }
    }
}
