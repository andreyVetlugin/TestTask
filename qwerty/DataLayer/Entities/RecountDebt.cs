using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Entities
{
    public class RecountDebt : IBenefitsEntity
    {
        public Guid Id { get; set; }
        public Guid PersonInfoRootId { get; set; }
        public decimal Debt { get; set; } //Отрицательное, если должен человек
        public decimal MonthlyPay { get; set; }
        public DateTime? LastTimePay { get; set; }
    }


    public static class RecountDebtQueryableExtensions
    {
        public static IQueryable<RecountDebt> ByPersonRootId(this IQueryable<RecountDebt> debts,
            Guid PersonInfoRootId)
        {
            return debts.Where(p => p.PersonInfoRootId == PersonInfoRootId);
        }

        public static IQueryable<RecountDebt> ByPersonRootIds(this IQueryable<RecountDebt> debts,
            IEnumerable<Guid> PersonInfoRootIds)
        {
            return debts.Where(p => PersonInfoRootIds.Contains(p.PersonInfoRootId));
        }
    }
}
