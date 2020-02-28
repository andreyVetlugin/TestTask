using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Entities.EGISSO
{
    public class PeriodType: IBenefitsEntity
    {
        public Guid Id { get; set; }
        public int PpNumber { get; set; }
        public string PositionCode { get; set; }
        public string Value { get; set; }
    }

    public static class EgissoPeriodTypeQueryableExtensions
    {
        public static IQueryable<PeriodType> ById(this IQueryable<PeriodType> types, Guid id) =>
            types.Where(r => r.Id == id);

        public static IQueryable<PeriodType> ByIds(this IQueryable<PeriodType> types, IEnumerable<Guid> ids) =>
            types.Where(r => ids.Contains(r.Id));
    }
}
