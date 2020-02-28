using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Entities
{
    public class ExtraPay : IBenefitsEntity
    {
        public Guid Id { get; set; }
        public Guid PersonRootId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? OutDate { get; set; }

        public Guid VariantId { get; set; }

        public decimal UralMultiplier { get; set; }
        public decimal Salary { get; set; }
        public decimal Premium { get; set; }
        public decimal MaterialSupport { get; set; }
        public decimal Perks { get; set; }
        public decimal Vysluga { get; set; }
        public decimal Secrecy { get; set; }
        public decimal Qualification { get; set; }
        public decimal GosPension { get; set; }
        public decimal ExtraPension { get; set; }
        public decimal Ds { get; set; }
        public decimal DsPerc { get; set; }
        public decimal TotalExtraPay { get; set; }
        public decimal TotalPension { get; set; }
        public decimal TotalPensionAndExtraPay { get; set; }
    }

    public static class ExtraPayQueryableExtensions
    {
        public static IQueryable<ExtraPay> ByPersonRootId(this IQueryable<ExtraPay> query, Guid personRootId) =>
            query.Where(p => p.PersonRootId == personRootId);
        public static IQueryable<ExtraPay> ActualByPersonRootId(this IQueryable<ExtraPay> extraPays, Guid personRootId) =>
            extraPays.Where(p => !p.OutDate.HasValue && p.PersonRootId == personRootId);

        public static IQueryable<ExtraPay> ActualByPersonRootIds(this IQueryable<ExtraPay> extraPays, IEnumerable<Guid> personRootIds) =>
           extraPays.Where(p => personRootIds.Contains(p.PersonRootId) && !p.OutDate.HasValue);

        public static IQueryable<ExtraPay> Actual(this IQueryable<ExtraPay> extraPays) =>
            extraPays.Where(p => !p.OutDate.HasValue);

        public static IQueryable<ExtraPay> ByVariantId(this IQueryable<ExtraPay> extraPays, Guid variantId) =>
            extraPays.Where(p => p.VariantId == variantId);
        public static IQueryable<ExtraPay> ByVariantIds(this IQueryable<ExtraPay> query, IEnumerable<Guid> variantIds) =>
            query.Where(p => variantIds.Contains(p.VariantId));

        public static IQueryable<ExtraPay> ByCreateYear(this IQueryable<ExtraPay> extraPays, int year) =>
            extraPays.Where(p => p.CreateDate.Year == year);
    }
}
