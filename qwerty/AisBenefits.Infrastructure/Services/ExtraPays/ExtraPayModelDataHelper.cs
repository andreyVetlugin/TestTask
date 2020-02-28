using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Infrastructure.Services.ExtraPayVariants;
using AisBenefits.Infrastructure.Services.WorkInfos;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AisBenefits.Infrastructure.Services.ExtraPays
{
    using ExtraPayModelDataResult = ModelDataResult<ExtraPayModelData>;
    using ExtraPaysModelDataResult = ModelDataResult<List<ExtraPayModelData>>;

    public static class ExtraPayModelDataHelper
    {
        public static ExtraPayModelDataResult GetExtraPayModelData(this IReadDbContext<IBenefitsEntity> readDbContext, Guid personInfoId)
        {
            var personInfo = readDbContext.Get<PersonInfo>()
                .ByRootId(personInfoId)
                .FirstOrDefault();
            var workInfosResult = readDbContext.GetWorkInfosModelData(personInfo);

            return readDbContext.GetExtraPayModelData(workInfosResult);
        }

        public static ExtraPayModelDataResult GetExtraPayModelData(this IReadDbContext<IBenefitsEntity> readDbContext, PersonInfo personInfo)
        {
            var workInfosResult = readDbContext.GetWorkInfosModelData(personInfo);

            return readDbContext.GetExtraPayModelData(workInfosResult);
        }

        public static ExtraPayModelDataResult GetExtraPayModelData(this IReadDbContext<IBenefitsEntity> readDbContext, ModelDataResult<WorkInfosModelData> workInfosResult)
        {
            if (!workInfosResult.Ok)
                return ExtraPayModelDataResult.BuildErrorFrom(workInfosResult);

            var result = readDbContext.GetExtraPaysModelData(new[] { workInfosResult.Data });

            if (!result.Ok)
                return ExtraPayModelDataResult.BuildErrorFrom(result);
            var data = result.Data[0];

            return ExtraPayModelDataResult.BuildSucces(data);
        }

        public static ExtraPaysModelDataResult GetExtraPaysModelData(this IReadDbContext<IBenefitsEntity> readDbContext, IReadOnlyList<Guid> personInfoIds)
        {
            var personInfos = readDbContext.Get<PersonInfo>()
                .ByRootIds(personInfoIds)
                .ToList();
            var workInfosResult = readDbContext.GetWorkInfosModelData(personInfos);

            if (!workInfosResult.Ok)
                return ExtraPaysModelDataResult.BuildErrorFrom(workInfosResult);

            return readDbContext.GetExtraPaysModelData(workInfosResult.Data);
        }

        public static ExtraPaysModelDataResult GetExtraPaysModelData(this IReadDbContext<IBenefitsEntity> readDbContext, IReadOnlyList<PersonInfo> personInfos)
        {
            var workInfosResult = readDbContext.GetWorkInfosModelData(personInfos);

            if (!workInfosResult.Ok)
                return ExtraPaysModelDataResult.BuildErrorFrom(workInfosResult);

            return readDbContext.GetExtraPaysModelData(workInfosResult.Data);
        }

        public static ExtraPaysModelDataResult GetExtraPaysModelData(this IReadDbContext<IBenefitsEntity> readDbContext, IReadOnlyList<WorkInfosModelData> workInfos)
        {
            var personInfoRootIds = workInfos.Select(w => w.PersonInfoRootId).ToList();

            var extraPays = readDbContext.Get<ExtraPay>()
                .ActualByPersonRootIds(personInfoRootIds)
                .ToDictionary(p => p.PersonRootId);

            var variants = readDbContext.Get<ExtraPayVariant>()
                .ToDictionary(v => v.Id);

            var registrationDates = readDbContext.Get<PersonInfo>()
                .ByIds(workInfos.Where(w => !w.PersonInfo.IsRoot()).Select(w => w.PersonInfo.RootId).Distinct())
                .Select(p => new
                {
                    p.RootId,
                    p.CreateTime.Date
                })
                .AsEnumerable()
                .Concat(
                    workInfos
                        .Where(w => w.PersonInfo.IsRoot())
                        .Select(w => new
                        {
                            w.PersonInfo.RootId,
                            w.PersonInfo.CreateTime.Date
                        }))
                .ToDictionary(p => p.RootId, p => p.Date);

            var dsPercs = readDbContext.Get<DsPerc>()
                .AsEnumerable()
                .GroupBy(p => p.Period.Year)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(p => p.Period).ThenByDescending(p => p.AgeDays).ToList());

            foreach (var registrationDate in registrationDates.Values)
            {
                if (!dsPercs.ContainsKey(registrationDate.Year))
                    return ExtraPaysModelDataResult.BuildInnerStateError($"Справочники %МДС для {registrationDate.Year} года не заполнены");
            }

            var minExtra = readDbContext.Get<MinExtraPay>().OrderByDescending(c => c.Date).FirstOrDefault();

            if (minExtra == null)
                return ExtraPaysModelDataResult.BuildInnerStateError($"Справочники Минимальной доплаты не заполнены");

            var data = workInfos.Select(w =>
                {
                    var extraPay = extraPays.GetValueOrDefault(w.PersonInfoRootId);
                    var initial = extraPay == null;

                    if (initial)
                        extraPay = new ExtraPay
                        {
                            Id = Guid.Empty,
                            PersonRootId = w.PersonInfoRootId,
                        };
                    var personInfo = w.PersonInfo;
                    var registrationDate = registrationDates[personInfo.RootId];
                    var genderType = SexHelper.IsMale(personInfo.Sex)
                        ? DsPercGenderType.Male
                        : SexHelper.IsFemale(personInfo.Sex)
                            ? DsPercGenderType.Female
                            : throw new InvalidOperationException();

                    var variant = initial ?
                        ExtraPayVariantHelper.CreateDefault()
                        : extraPay.VariantId == Guid.Empty
                        ? ExtraPayVariantHelper.CreateDefault(
                            extraPay.Premium,
                            extraPay.Salary,
                            extraPay.UralMultiplier,
                            extraPay.MaterialSupport)
                        : variants.GetValueOrDefault(extraPay.VariantId);

                    var dsPerc = dsPercs[registrationDate.Year]
                        .FirstOrDefault(p => (p.GenderType == DsPercGenderType.General || p.GenderType == genderType) && p.Period <= registrationDate && p.AgeDays <= w.WorkAge.AgeDays)
                        ??
                        new DsPerc
                        {
                            Period = new DateTime(registrationDate.Year, 1, 1),
                            Amount = extraPay.DsPerc
                        };

                    return new ExtraPayModelData(extraPay, initial, variant, w.WorkAge.AgeDays, dsPerc, minExtra);
                })
                .ToList();

            return ExtraPaysModelDataResult.BuildSucces(data);
        }
    }

    public class ExtraPayModelData
    {
        public ExtraPay Instance { get; }
        public bool Initial { get; }

        public ExtraPayVariant Variant { get; }
        public int WorkAgeDays { get; }
        public DsPerc DsPerc { get; }
        public MinExtraPay MinExtraPay { get; }

        public ExtraPayModelData(ExtraPay instance, bool initial, ExtraPayVariant variant, int workAgeDays, DsPerc dsPerc, MinExtraPay minExtraPay)
        {
            Instance = instance;
            Initial = initial;
            Variant = variant;
            WorkAgeDays = workAgeDays;
            DsPerc = dsPerc;
            MinExtraPay = minExtraPay;
        }
    }
}
