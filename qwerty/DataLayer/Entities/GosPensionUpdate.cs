using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Entities
{
    public class GosPensionUpdate: IBenefitsEntity
    {
        public Guid Id { get; set; }
        public Guid PersonInfoRootId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; } 
        public bool Approved { get; set; }
        public bool Declined { get; set; }

        public GosPensionUpdateState State { get; set; }
    }

    public enum GosPensionUpdateState
    {
        Process,
        Success,
        Error
    }

    public static class IncomePensionQueryableExtensions
    {
        public static IQueryable<GosPensionUpdate> ById(this IQueryable<GosPensionUpdate> incomePensions, Guid id) =>
            incomePensions.Where(p => p.Id == id);

        public static IQueryable<GosPensionUpdate> NotDeclinedAndNotApproved(this IQueryable<GosPensionUpdate> incomePensions) =>
            incomePensions.Where(p => p.Approved==false&&p.Declined==false);

        public static IQueryable<GosPensionUpdate> ByIds(this IQueryable<GosPensionUpdate> incomePensions, Guid[] ids) =>
            incomePensions.Where(p =>ids.Contains(p.Id));

        public static IQueryable<GosPensionUpdate> SuccessActualAt(this IQueryable<GosPensionUpdate> incomePensions, int year, int month) =>
            incomePensions.Where(p => p.Date.Year == year && p.Date.Month == month && p.State == GosPensionUpdateState.Success && !(p.Approved || p.Declined));

        public static IQueryable<GosPensionUpdate> ActualAtByPersonInfoRootId(this IQueryable<GosPensionUpdate> incomePensions, int year, int month, Guid personInfoRootId) =>
            incomePensions.Where(p => p.Date.Year == year && p.Date.Month == month && p.PersonInfoRootId == personInfoRootId);

        public static IQueryable<GosPensionUpdate> ActualAt(this IQueryable<GosPensionUpdate> incomePensions, int year, int month) =>
            incomePensions.Where(p => p.Date.Year == year && p.Date.Month == month);
    }
}
