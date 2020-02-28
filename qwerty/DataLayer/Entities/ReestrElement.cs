using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataLayer.Entities
{
    public class ReestrElement: IBenefitsEntity
    {
        public Guid Id { get; set; }
        public Guid ReestrId { get; set; }

        public Guid PersonInfoRootId { get; set; }
        public Guid PersonInfoId { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public decimal BaseSumm { get; set; }
        public decimal Summ { get; set; }

        public Guid? PersonBankCardId { get; set; }

        [MaxLength(256)]
        public string Comment { get; set; }

        public bool Deleted { get; set; }
    }


    public static class ReestrElementQueryableExtensions
    {
        public static IQueryable<ReestrElement> ExcludeReestrs(this IQueryable<ReestrElement> reestrs,
            IEnumerable<Guid> reestrIds) =>
            reestrs.Where(p => !reestrIds.Contains(p.ReestrId));

        public static IQueryable<ReestrElement> ByReestrId(this IQueryable<ReestrElement> reestrs, Guid reestrId) =>
            reestrs.Where(p => p.ReestrId == reestrId && !p.Deleted);

        public static IQueryable<ReestrElement> ByReestrIds(this IQueryable<ReestrElement> reestrs, IEnumerable<Guid> reestrIds) =>
            reestrs.Where(p => reestrIds.Contains(p.ReestrId) && !p.Deleted);

        public static IQueryable<ReestrElement> ByReestrIdsIncludeDeleted(this IQueryable<ReestrElement> reestrs, IEnumerable<Guid> reestrIds) =>
            reestrs.Where(p => reestrIds.Contains(p.ReestrId));

        public static IQueryable<ReestrElement> ById(this IQueryable<ReestrElement> reestrs, Guid id) =>
            reestrs.Where(p => p.Id == id);

        public static IQueryable<ReestrElement> ByPersonInfoRootId(this IQueryable<ReestrElement> reestrs, Guid personInfoRootId) =>
            reestrs.Where(p => p.PersonInfoRootId == personInfoRootId && !p.Deleted);

        public static IQueryable<ReestrElement> ByPersonInfoRootIdAndReestrIds(this IQueryable<ReestrElement> reestrs, Guid personInfoRootId, List<Guid> reestrIds) =>
            reestrs.Where(p => p.PersonInfoRootId == personInfoRootId && !p.Deleted && reestrIds.Contains(p.ReestrId));

    }
}
