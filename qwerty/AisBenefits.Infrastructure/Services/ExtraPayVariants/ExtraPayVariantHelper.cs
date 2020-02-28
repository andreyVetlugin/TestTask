using AisBenefits.Infrastructure.Helpers;
using DataLayer.Entities;
using System;

namespace AisBenefits.Infrastructure.Services.ExtraPayVariants
{
    public  static class ExtraPayVariantHelper
    {
        public static ExtraPayVariant CreateDefault()
        {
            return new ExtraPayVariant
            {
                Id = Guid.Empty,
                Number = 0,
                MatSupportMultiplier = .166m,
                PremiumPerc = 50m,
                UralMultiplier = 1.15m,
                VyslugaDivPerc = 40m,
                VyslugaMultiplier = .4m,
                IgnoreGosPension = false
            };
        }
        public static ExtraPayVariant CreateDefault(
            decimal premium,
            decimal salary,
            decimal uralMultiplier,
            decimal materialSupport
            )
        {
            return new ExtraPayVariant
            {
                Id = Guid.Empty,
                Number = 0,
                MatSupportMultiplier = CalcHelper.MatSupportMultiplier(materialSupport, salary),
                PremiumPerc = CalcHelper.PremiumPerc(premium, salary, uralMultiplier),
                UralMultiplier = uralMultiplier,
                VyslugaDivPerc = 40m,
                VyslugaMultiplier = .4m,
                IgnoreGosPension = false
            };
        }

        public static bool IsDefault(ExtraPayVariant variant)
        {
            return variant.Number == 0;
        }
    }
}
