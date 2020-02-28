using AisBenefits.Infrastructure.Services.ExtraPays;
using DataLayer.Containers;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Linq;
using AisBenefits.Infrastructure.Services.WorkInfos;

namespace AisBenefits.Infrastructure.Services
{
    public interface IExtraPayCountService
    {
        ExtraPayContainer Count(Guid personInfoRootId);        
    }

    public class ExtraPayCountService: IExtraPayCountService
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public ExtraPayCountService(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public ExtraPayContainer Count(Guid personInfoRootId)
        {
            var personInfo = readDbContext.Get<PersonInfo>()
                .ByRootId(personInfoRootId)
                .FirstOrDefault();
            var workInfosResult = readDbContext.GetWorkInfosModelData(personInfo);
            var result = readDbContext.GetExtraPayModelData(workInfosResult);
            if (!result.Ok)
                throw new InvalidOperationException();

            var model = result.Data.BuildModel();

            return new ExtraPayContainer
            {
                Ds = model.Ds,
                DsPerc = model.DsPerc,
                MaterialSupport = model.MaterialSupport,
                Premium = model.Premium,
                SalaryMultiplied = model.SalaryMultiplied,
                TotalExtraPay = model.TotalExtraPay,
                TotalPension = model.TotalPension,
                TotalPensionAnExtraPay = model.TotalPensionAndExtraPay,
                UralMultiplier = model.UralMultiplier,
                Vysluga = model.Vysluga,
                VyslugaMultiplier = model.VyslugaMultiplier,
                WorkAge = model.WorkAgeDays

            };
        }
    }
}
