using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AisBenefits.Infrastructure.Services.Recounts
{
    public class RecountDebtModelData
    {
        public RecountDebt RecountDebt;
        public decimal CurrentPay { get; set; }
        public bool IsCompletedInThisMonth { get;}

        public RecountDebtModelData(RecountDebt recountDebt)
        {
            if (recountDebt == null) CurrentPay = 0;
            else
            {
                RecountDebt = recountDebt;
                CurrentPay = Math.Abs(RecountDebt.MonthlyPay) > Math.Abs(RecountDebt.Debt)
                    ? RecountDebt.Debt
                    : RecountDebt.MonthlyPay;
                if (recountDebt.LastTimePay.HasValue)
                    if (recountDebt.LastTimePay.Value.Month == DateTime.Now.Month)
                    {
                        CurrentPay = 0;
                        IsCompletedInThisMonth = true;
                    }
            }

        }
    }

    public static class RecounDebtModelDataHelper
    {
        public static ModelDataResult<RecountDebtModelData> GetRecountDebtModelDataResult(
            this IReadDbContext<IBenefitsEntity> readDbContext, Guid personInfoRootId)
        {
            var recountDebt = readDbContext.Get<RecountDebt>().ByPersonRootId(personInfoRootId).FirstOrDefault();

            return ModelDataResult<RecountDebtModelData>.BuildSucces(new RecountDebtModelData(recountDebt));
        }

        public static List<ModelDataResult<RecountDebtModelData>> GetRecountDebtModelDataResult(
            this IReadDbContext<IBenefitsEntity> readDbContext, IEnumerable<Guid> personInfoRootIds)
        {
            var result = new List<ModelDataResult<RecountDebtModelData>>();
            var debtsList = readDbContext.Get<RecountDebt>().ByPersonRootIds(personInfoRootIds).ToList();

            foreach (var debt in debtsList)
            {
                var res = ModelDataResult<RecountDebtModelData>.BuildSucces(new RecountDebtModelData(debt));
                result.Add(res);
            }

            return result;
        }
    }
}
