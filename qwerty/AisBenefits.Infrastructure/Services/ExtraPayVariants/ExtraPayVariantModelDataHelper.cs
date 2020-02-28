using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.ExtraPayVariants
{
    using ExtraPayVariantModelDataResult = ModelDataResult<ExtraPayVariantModelData>;
    using ExtraPayVariantsModelDataResult = ModelDataResult<List<ExtraPayVariantModelData>>;

    public static class ExtraPayVariantModelDataHelper
    {
        public static ExtraPayVariantModelDataResult GetExtraPayVariantModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, Guid variantId)
        {
            var variant = readDbContext.Get<ExtraPayVariant>()
                .ById(variantId)
                .FirstOrDefault();

            if(variant == null)
                return ExtraPayVariantModelDataResult.BuildNotExist("Указанный вариант не существует");

            var hasUsages = readDbContext.Get<ExtraPay>()
                .ByVariantId(variantId)
                .Any();

            return ExtraPayVariantModelDataResult.BuildSucces(new ExtraPayVariantModelData(variant, hasUsages));
        }

        public static ExtraPayVariantsModelDataResult GetAllExtraPayVariantsModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext)
        {
            var variants = readDbContext.Get<ExtraPayVariant>().ToList();

            var usages = readDbContext.Get<ExtraPay>()
                .GroupBy(p => p.VariantId)
                .Select(p => p.Key)
                .ToHashSet();

            return ExtraPayVariantsModelDataResult.BuildSucces(
                variants.Select(v => new ExtraPayVariantModelData(v, usages.Contains(v.Id))).ToList()
                );
        }
    }

    public class ExtraPayVariantModelData
    {
        public ExtraPayVariant Variant { get; }
        public bool HasUsages { get; }

        public ExtraPayVariantModelData(ExtraPayVariant variant, bool hasUsages)
        {
            Variant = variant;
            HasUsages = hasUsages;
        }
    }
}
