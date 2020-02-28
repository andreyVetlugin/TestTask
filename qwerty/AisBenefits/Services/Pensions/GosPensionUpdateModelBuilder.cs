using AisBenefits.Models.GosPensions;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System.Collections.Generic;
using System.Linq;

namespace AisBenefits.Services.Pensions
{
    public interface IGosPensionUpdateModelBuilder
    {
        List<GosPensionUpdateModel> Build(List<GosPensionUpdate> gosPensionUpdateList);
    }


    public class GosPensionUpdateModelBuilder : IGosPensionUpdateModelBuilder
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public GosPensionUpdateModelBuilder(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public List<GosPensionUpdateModel> Build(List<GosPensionUpdate> gosPensionUpdateList)
        {
            var personInfos = readDbContext
                .Get<PersonInfo>()
                .ByRootIds(gosPensionUpdateList.Select(u => u.PersonInfoRootId))
                .ToDictionary(p => p.RootId);

            var extraPays = readDbContext
                .Get<ExtraPay>()
                .ActualByPersonRootIds(gosPensionUpdateList.Select(u => u.PersonInfoRootId))
                .ToDictionary(p => p.PersonRootId);

            return gosPensionUpdateList
                .Select(u => (update: u, personInfo: personInfos[u.PersonInfoRootId], extraPay: extraPays[u.PersonInfoRootId]))
                .OrderBy(u => u.personInfo.Number)
                .Select((u, i) =>
                {
                    var (update, personInfo, extraPay) = u;

                    return new GosPensionUpdateModel
                    {
                        Number = i + 1,
                        FIO = $"{personInfo.SurName} {personInfo.Name} {personInfo.MiddleName}",
                        CurrentPension = extraPay.GosPension,
                        GosPensionUpdateId = update.Id,
                        NewPensionValue = update.Amount
                    };
                })
                .ToList();
        }
    }
}
