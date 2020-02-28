using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Entities
{
    public class WorkInfo: IBenefitsEntity
    {
        public Guid Id { get; set; }
        public Guid RootId { get; set; }
        public Guid? NextId { get; set; }
        public Guid PersonInfoRootId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? OutdateTime { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid FunctionId { get; set; }
    }



    public static class WorkInfoQueryableExtensions
    {
        public static IQueryable<WorkInfo> ByPersonInfoId(this IQueryable<WorkInfo> workInfos, Guid personInfoId) =>
            workInfos.Where(r => r.PersonInfoRootId == personInfoId&& r.NextId==null);
        public static IQueryable<WorkInfo> ByPersonInfoIds(this IQueryable<WorkInfo> workInfos, IEnumerable<Guid> personInfoIds) =>
            workInfos.Where(r => personInfoIds.Contains(r.PersonInfoRootId) && r.NextId == null);
        public static IQueryable<WorkInfo> ActualByRootId(this IQueryable<WorkInfo> workInfos, Guid rootId) =>
            workInfos.Where(r => r.RootId == rootId&&r.NextId==null);
        public static IQueryable<WorkInfo> AllActual(this IQueryable<WorkInfo> workInfos) =>
            workInfos.Where(r => r.NextId == null);

        public static IQueryable<WorkInfo> ByFunctionId(this IQueryable<WorkInfo> workInfos, Guid functionId) =>
            workInfos.Where(r => r.FunctionId == functionId);
        public static IQueryable<WorkInfo> ActualByFunctionIds(this IQueryable<WorkInfo> workInfos, IEnumerable<Guid> functionIds) =>
            workInfos.Where(r => r.NextId == null && functionIds.Contains(r.FunctionId));

        public static IQueryable<WorkInfo> ByOrganizationId(this IQueryable<WorkInfo> workInfos, Guid organizationId) =>
            workInfos.Where(r => r.OrganizationId == organizationId);
    }


}
