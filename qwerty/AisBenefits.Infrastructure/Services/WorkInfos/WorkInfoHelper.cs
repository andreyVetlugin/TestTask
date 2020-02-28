using System;
using System.Collections.Generic;
using System.Linq;
using AisBenefits.Infrastructure.Helpers;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.WorkInfos
{
    using WorkInfoModelDataResult = ModelDataResult<WorkInfoModelData>;
    using WorkInfosModelDataResult = ModelDataResult<WorkInfosModelData>;

    public static class WorkInfoHelper
    {
        public static WorkInfoModelDataResult GetWorkInfoModelData(this IReadDbContext<IBenefitsEntity> readDbContext,
            Guid rootId)
        {
            var workInfo = readDbContext.Get<WorkInfo>().ActualByRootId(rootId).FirstOrDefault();

            if(workInfo == null)
                return WorkInfoModelDataResult.BuildNotExist("Указанный стаж не существует");

            var organization = readDbContext.Get<Organization>()
                .ById(workInfo.OrganizationId)
                .FirstOrDefault();

            return WorkInfoModelDataResult.BuildSucces(
                new WorkInfoModelData(workInfo, organization)
                );
        }

        public static ModelDataResult<List<WorkInfosModelData>> GetWorkInfosModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, IReadOnlyList<PersonInfo> personInfos)
        {
            var workInfos = readDbContext.Get<WorkInfo>()
                .ByPersonInfoIds(personInfos.Select(p => p.RootId))
                .AsEnumerable()
                .GroupBy(w => w.PersonInfoRootId)
                .ToDictionary(g => g.Key);

            var organizations = readDbContext.Get<Organization>()
                .ToDictionary(o => o.Id);

            var data = personInfos
                .Select(p =>
                    new WorkInfosModelData(
                        p,
                        (workInfos.GetValueOrDefault(p.RootId) ?? Enumerable.Empty<WorkInfo>())
                        .Select(w => new WorkInfoModelData(w, organizations[w.OrganizationId]))
                        .ToList()))
                .ToList();

            return ModelDataResult<List<WorkInfosModelData>>.BuildSucces(data);
        }

        public static WorkInfosModelDataResult GetWorkInfosModelData(this IReadDbContext<IBenefitsEntity> readDbContext,
            PersonInfo personInfo)
        {
            var workInfos = readDbContext.Get<WorkInfo>()
                .ByPersonInfoId(personInfo.RootId)
                .ToList();

            var organizations = readDbContext.Get<Organization>()
                .ByIds(workInfos.Select(w => w.OrganizationId).Distinct())
                .ToDictionary(o => o.Id);

            return WorkInfosModelDataResult.BuildSucces(
                new WorkInfosModelData(personInfo,
                    workInfos
                        .Select(w => new WorkInfoModelData(w, organizations[w.OrganizationId]))
                        .ToList())
            );
        }

        public static WorkInfosModelDataResult GetWorkInfosModelData(this IReadDbContext<IBenefitsEntity> readDbContext,
            WorkInfo workInfo, WorkInfosModelDataType type, Organization organization = null)
        {
            var workInfos = readDbContext.Get<WorkInfo>()
                .ByPersonInfoId(workInfo.PersonInfoRootId)
                .ToList();
            var personInfo = readDbContext.Get<PersonInfo>()
                .ById(workInfo.PersonInfoRootId)
                .FirstOrDefault();

            switch (type)
            {
                case WorkInfosModelDataType.Pure:
                    break;
                case WorkInfosModelDataType.Add:
                    workInfos.Add(workInfo);
                    break;
                case WorkInfosModelDataType.Edit:
                    workInfos.Remove(workInfos.Find(w => w.RootId == workInfo.RootId));
                    workInfos.Add(workInfo);
                    break;
                case WorkInfosModelDataType.Remove:
                    workInfos.Remove(workInfos.Find(w => w.RootId == workInfo.RootId));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var organizations = readDbContext.Get<Organization>()
                .ByIds(workInfos.Select(w => w.OrganizationId).Distinct())
                .ToDictionary(o => o.Id);

            if(type == WorkInfosModelDataType.Add || type == WorkInfosModelDataType.Edit)
                organizations[organization.Id] = organization;

            return WorkInfosModelDataResult.BuildSucces(
                new WorkInfosModelData(personInfo,
                    workInfos
                        .Select(w => new WorkInfoModelData(w, organizations[w.OrganizationId]))
                        .ToList())
            );
        }
    }

    public enum WorkInfosModelDataType
    {
        Pure,
        Add,
        Edit,
        Remove
    }

    public class WorkInfoModelData
    {
        public WorkInfo Instance { get; }

        public Organization Organization { get; }

        public WorkAge WorkAge => new WorkAge(Instance.StartDate, Instance.EndDate) * Organization.Multiplier;

        public WorkInfoModelData(WorkInfo instance, Organization organization)
        {
            Instance = instance;
            Organization = organization;
        }
    }

    public class WorkInfosModelData
    {
        public PersonInfo PersonInfo { get; }
        public List<WorkInfoModelData> WorkInfos { get; }
        public Guid PersonInfoRootId => PersonInfo.RootId;

        public WorkAge WorkAge =>
            GetWorkAge(WorkInfos) +
            GetWorkAge(WorkInfos.Where(w => w.Organization.Multiplier == 2));

        static WorkAge GetWorkAge(IEnumerable<WorkInfoModelData> workInfos) =>
            workInfos.OrderBy(w => w.Instance.StartDate)
                .Select(w => (workAge: new WorkAge(w.Instance.StartDate, w.Instance.EndDate), start: w.Instance.StartDate, end: w.Instance.EndDate))
                .DefaultIfEmpty()
                .Aggregate((l, r) => (
                        l.workAge + r.workAge - (r.start < l.end ? new WorkAge(r.start, l.end) : new WorkAge(0)),
                        r.start,
                        new DateTime(Math.Max(l.end.Ticks, r.end.Ticks))
                    )
                )
                .workAge;

        public WorkInfosModelData(PersonInfo personInfo, List<WorkInfoModelData> workInfos)
        {
            PersonInfo = personInfo;
            WorkInfos = workInfos;
        }
    }
}
