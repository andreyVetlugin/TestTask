using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.DsPercs
{
    using DsPercModelDataResult = ModelDataResult<DsPercModelData>;
    using DsPercsModelDataResult = ModelDataResult<List<DsPercModelData>>;

    public static class DsPercModelDataHelper
    {
        public static DsPercModelDataResult GetDsPercModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, Guid dsPecrId)
        {
            var dsPerc = readDbContext.Get<DsPerc>()
                .ById(dsPecrId)
                .FirstOrDefault();

            if(dsPerc == null)
                return DsPercModelDataResult.BuildNotExist("Указанный %МДС не существует");

            var hasUsages = readDbContext.Get<ExtraPay>()
                .ByCreateYear(dsPerc.Period.Year)
                .Any();

            var yearEditAllow = readDbContext
                .Get<DsPerc>()
                .ByDate(dsPerc.Period)
                .All(p => p.GenderType == DsPercGenderType.General) &&
                !hasUsages;

            return DsPercModelDataResult.BuildSucces(new DsPercModelData(dsPerc, hasUsages, yearEditAllow));
        }

        public static DsPercsModelDataResult GetAllDsPercsModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext)
        {
            var dsPercs = readDbContext.Get<DsPerc>().ToList();

            var usages = readDbContext.Get<ExtraPay>()
                .GroupBy(p => p.CreateDate.Year)
                .Select(p => p.Key)
                .ToHashSet();

            var yearEditAllow = dsPercs
                .GroupBy(p => p.Period.Year)
                .Where(g => g.All(p => p.GenderType == DsPercGenderType.General))
                .Select(g => g.Key)
                .ToHashSet();

            return DsPercsModelDataResult.BuildSucces(
                dsPercs.Select(v => new DsPercModelData(v, usages.Contains(v.Period.Year), yearEditAllow.Contains(v.Period.Year) && !usages.Contains(v.Period.Year))).ToList()
                );
        }
    }

    public class DsPercModelData
    {
        public DsPerc DsPerc { get; }
        public bool HasUsages { get; }
        public bool AllowEdit { get; }

        public DsPercModelData(DsPerc dsPerc, bool hasUsages, bool allowEdit)
        {
            DsPerc = dsPerc;
            HasUsages = hasUsages;
            AllowEdit = allowEdit;
        }
    }
}
