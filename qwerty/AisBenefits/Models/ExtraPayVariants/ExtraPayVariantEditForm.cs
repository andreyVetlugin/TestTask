using System;
using System.ComponentModel.DataAnnotations;
using AisBenefits.Infrastructure.Services.ExtraPayVariants;
using DataLayer.Entities;

namespace AisBenefits.Models.ExtraPayVariants
{
    public class ExtraPayVariantEditForm : IExtraPayVariantEditForm
    {
        public Guid? VariantId { get; set; }

        public int Number { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? UralMultiplier { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? PremiumPerc { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? MatSupportMultiplier { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? VyslugaMultiplier { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? VyslugaDivPerc { get; set; }
        public bool IgnoreGosPension { get; set; }
    }
}
