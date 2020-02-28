using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using AisBenefits.Infrastructure.Helpers;

namespace AisBenefits.Infrastructure.Services.DsPercs
{
    using DsPercModelDataResult = ModelDataResult<DsPercModelData>;
    using DsPercEditYearModelDataResult = ModelDataResult<DsPercEditYearModelData>;

    public static class DsPercOperationHelper
    {
        public static OperationResult EditDsPercYear(this IWriteDbContext<IBenefitsEntity> writeDbContext,
            DsPercEditYearModelDataResult modelDataResult)
        {
            if(!modelDataResult.Ok)
                return OperationResult.BuildErrorFrom(modelDataResult);

            var form = modelDataResult.Data.Form;
            var dsPercs = modelDataResult.Data.DsPercs;

            foreach (var dsPerc in dsPercs)
            {
                writeDbContext.Remove(dsPerc);
            }

            foreach (var f in form.DsPercs)
            {
                var dsPerc = new DsPerc
                {
                    Id = Guid.NewGuid(),
                    Period = new DateTime(form.Year, 1, 1),
                    Amount = f.Amount,
                    AgeDays = WorkAge.GetAgeDays(f.AgeYears, f.AgeMonths, f.AgeDays)
                };
                writeDbContext.Add(dsPerc);
            }

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        public static OperationResult DeleteDsPerc(
            this IWriteDbContext<IBenefitsEntity> writeDbContext, DsPercModelDataResult modelDataResult)
        {
            if(!modelDataResult.Ok)
                return OperationResult.BuildErrorFrom(modelDataResult);

            var data = modelDataResult.Data;

            if(data.DsPerc.Period.Year < DateTime.Now.Year)
               return OperationResult.BuildFormError("Указанный год не доступен для редактирования");

            if(!data.AllowEdit)
                return OperationResult.BuildInnerStateError("Указанный %МДС используется в системе");

            writeDbContext.Remove(data.DsPerc);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        public static DsPercEditYearModelDataResult BuildDsPercsEditModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, IDsPercEditYearForm form)
        {
            if (!Enumerable.SequenceEqual(
                form.DsPercs.OrderBy(p => p.Amount),
                form.DsPercs.OrderBy(p => (p.AgeYears, p.AgeMonths, p.AgeDays))))
                return DsPercEditYearModelDataResult.BuildFormError("За наибольший стаж полагается наибольший процент");

            var dsPercs = readDbContext.Get<DsPerc>().ByDate(new DateTime(form.Year, 1, 1)).ToList();

            if (dsPercs.Count > 0 && form.Year < DateTime.Now.Year)
                return DsPercEditYearModelDataResult.BuildFormError("Указанный год не доступен для редактирования");


            return DsPercEditYearModelDataResult.BuildSucces(
                new DsPercEditYearModelData(form, dsPercs)
                );
        }
    }

    public class DsPercEditYearModelData
    {
        public IDsPercEditYearForm Form { get; }

        public List<DsPerc> DsPercs { get; }

        public DsPercEditYearModelData(IDsPercEditYearForm form, List<DsPerc> dsPercs)
        {
            Form = form;
            DsPercs = dsPercs;
        }
    }

    public interface IDsPercEditYearForm
    {
        int Year { get; }
        IReadOnlyList<IDsPercEditForm> DsPercs { get; }
    }

    public interface IDsPercEditForm
    {
        decimal Amount { get; }
        int AgeYears { get; }
        int AgeMonths { get; }
        int AgeDays { get; }
    }
}
