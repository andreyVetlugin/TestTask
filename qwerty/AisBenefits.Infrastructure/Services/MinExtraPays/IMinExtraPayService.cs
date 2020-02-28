using AisBenefits.Infrastructure.DTOs;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.MinExtraPays
{
    public interface IMinExtraPayService
    {
        void Edit(IWriteDbContext<IBenefitsEntity> writeDbContext, IMinExtraPayDto minExtraPayDto);
        MinExtraPay Get();
    }
}
