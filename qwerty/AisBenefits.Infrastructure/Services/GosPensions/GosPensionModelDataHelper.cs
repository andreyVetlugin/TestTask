using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.GosPensions
{
    using GosPensionModelDataResult = ModelDataResult<GosPensionModelData>;
    using GosPensionsModelDataResult = ModelDataResult<List<GosPensionModelData>>;

    public static class GosPensionModelDataHelper
    {
        public static GosPensionModelDataResult GetGosPensionUpdateModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, Guid personInfoRootId)
        {
            var personInfo = readDbContext.Get<PersonInfo>()
                .ByRootId(personInfoRootId)
                .FirstOrDefault();

            if (personInfo == null)
                return GosPensionModelDataResult.BuildNotExist("Указанная карта не существует");

            return readDbContext.GetGosPensionUpdateModelData(personInfo, DateTime.Now);
        }
        public static GosPensionModelDataResult GetGosPensionUpdateModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, PersonInfo personInfo, DateTime periodDate)
        {
            var solution = readDbContext.Get<Solution>().ByPersonRootId(personInfo.RootId).FirstOrDefault();

            if (solution == null)
                return GosPensionModelDataResult.BuildInnerStateError("Для указанного человека не определено решение");

            var date = periodDate;

            var update = readDbContext.Get<GosPensionUpdate>()
                .ActualAtByPersonInfoRootId(date.Year, date.Month, personInfo.RootId)
                .FirstOrDefault();

            var isNew = update == null;

            if (isNew)
            {
                update = new GosPensionUpdate
                {
                    Id = Guid.NewGuid(),
                    PersonInfoRootId = personInfo.RootId,
                    Date = date,
                    State = GosPensionUpdateState.Process
                };
            }

            return GosPensionModelDataResult.BuildSucces(
                new GosPensionModelData(personInfo, update, isNew)
                );
        }

        public static GosPensionsModelDataResult GetGosPensionUpdatesModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext,
            bool includeNew)
        {
            var personInfos =
                (from personInfo in readDbContext.Get<PersonInfo>().Active()
                 join solution in readDbContext.Get<Solution>().Positive()
                     on personInfo.RootId equals solution.PersonInfoRootId
                 group personInfo by personInfo.Id into g
                 select g.FirstOrDefault())
                .ToList();

            var date = DateTime.Now;
            var updates = readDbContext.Get<GosPensionUpdate>()
                .ActualAt(date.Year, date.Month)
                .ToDictionary(u => u.PersonInfoRootId);

            return GosPensionsModelDataResult.BuildSucces(personInfos
                .Where(p => includeNew || updates.ContainsKey(p.RootId))
                .Select(p =>
                    new GosPensionModelData(
                        p,
                        updates.GetValueOrDefault(p.RootId) ?? new GosPensionUpdate
                        {
                            Id = Guid.NewGuid(),
                            PersonInfoRootId = p.RootId,
                            Date = date,
                            State = GosPensionUpdateState.Process
                        },
                        !updates.ContainsKey(p.RootId)
                    ))
                .ToList());
        }
    }

    public class GosPensionModelData
    {
        public PersonInfo PersonInfo { get; }
        public GosPensionUpdate Update { get; }

        public bool New { get; }

        public GosPensionModelData(PersonInfo personInfo, GosPensionUpdate update, bool @new)
        {
            PersonInfo = personInfo;
            Update = update;
            New = @new;
        }

    }
}
