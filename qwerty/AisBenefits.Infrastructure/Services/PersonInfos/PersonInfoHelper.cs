using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.PersonInfos
{
    public static class PersonInfoHelper
    {
        public static HashSet<Guid> GetActivePersonInfoRootIds(this IReadDbContext<IBenefitsEntity> readDbContext)
        {
            var activeBySoution = readDbContext.GetPositivePersonInfoRootIds();

            var activeBySelf = readDbContext.ActiveApprovedPersonInfoRootIds();

            return Enumerable.Intersect(activeBySoution, activeBySelf).ToHashSet();
        }

        public static HashSet<Guid> GetMassRecalculateablePersonInfoRootIds(this IReadDbContext<IBenefitsEntity> read) =>
            Enumerable.Intersect(
                read.GetPositivePersonInfoRootIds(),
                read.Get<PersonInfo>().Active().Select(s => s.RootId).AsEnumerable()
                ).Intersect(
                read.Get<ExtraPay>().Actual().Select(e => e.PersonRootId).AsEnumerable()
                ).ToHashSet();

        public static List<Guid> GetPositivePersonInfoRootIds(this IReadDbContext<IBenefitsEntity> read) =>
            read.Get<Solution>()
                    .Positive()
                    .Select(s => s.PersonInfoRootId)
                    .Distinct()
                    .ToList();

        public static List<Guid> ActiveApprovedPersonInfoRootIds(this IReadDbContext<IBenefitsEntity> read) =>
            read.Get<PersonInfo>()
                .ActiveApproved()
                .Select(p => p.RootId)
                .Distinct()
                .ToList();
    }

}
