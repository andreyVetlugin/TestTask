using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataLayer.Entities
{
    public class ExtraPayVariant : IBenefitsEntity
    {
        [Key]
        public Guid Id { get; set; }
        public int Number { get; set; }

        public decimal? UralMultiplier { get; set; }

        public decimal? PremiumPerc { get; set; }
        public decimal? MatSupportMultiplier { get; set; }

        public decimal? VyslugaMultiplier { get; set; }
        public decimal? VyslugaDivPerc { get; set; }

        public bool IgnoreGosPension { get; set; }
    }

    public static class ExtraPayVariantQueryableExtensions
    {
        public static IQueryable<ExtraPayVariant> ById(this IQueryable<ExtraPayVariant> extraPayVariants, Guid id) =>
            extraPayVariants.Where(r => r.Id == id);
        public static IQueryable<ExtraPayVariant> ByNumber(this IQueryable<ExtraPayVariant> extraPayVariants, int number) =>
            extraPayVariants.Where(r => r.Number == number);
    }
}
