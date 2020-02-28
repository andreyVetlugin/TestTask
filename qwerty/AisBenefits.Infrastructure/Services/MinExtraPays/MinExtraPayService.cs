using System;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.DTOs;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.MinExtraPays
{
    public class MinExtraPayService : IMinExtraPayService
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly ILogBuilder logBuilder;

        public MinExtraPayService(IReadDbContext<IBenefitsEntity> readDbContext, ILogBuilder logBuilder)
        {
            this.readDbContext = readDbContext;
            this.logBuilder = logBuilder;
        }

        public void Edit(IWriteDbContext<IBenefitsEntity> writeDbContext, IMinExtraPayDto minExtraPayDto)
        {
            var newMinExtra = new MinExtraPay
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Value = minExtraPayDto.Value
            };
            writeDbContext.Add(newMinExtra);

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.MinExtraEdit, newMinExtra.Id);
            writeDbContext.Add(infoLog);
        }

        public MinExtraPay Get()
        {
            return readDbContext.Get<MinExtraPay>().OrderByDescending(c => c.Date).FirstOrDefault();
        }
    }
}
