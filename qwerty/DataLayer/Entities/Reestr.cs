using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Entities
{
    public class Reestr : IBenefitsEntity
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime InitDate { get; set; }
        public bool IsCompleted { get; set; }
        public int Number { get; set; }
        //public bool IsSaved { get; set; }


    }

    public static class ReestrQueryableExtensions
    {
        public static IQueryable<Reestr> ByDate(this IQueryable<Reestr> reestrs, DateTime date) =>
            reestrs.Where(r => r.Date.Month == date.Month && r.Date.Year == date.Year);

        public static IQueryable<Reestr> NotCompletedByDate(this IQueryable<Reestr> reestrs, DateTime date) =>
            reestrs.Where(p => p.Date.Month == date.Month && p.Date.Year == date.Year && !p.IsCompleted);

        public static IQueryable<Reestr> Completed(this IQueryable<Reestr> reestrs) =>
            reestrs.Where(p => p.IsCompleted);

        public static IQueryable<Reestr> NotCompleted(this IQueryable<Reestr> reestrs) =>
            reestrs.Where(p => !p.IsCompleted);

        public static IQueryable<Reestr> CompletedInDateInterval(this IQueryable<Reestr> reestrs, DateTime from, DateTime to) =>
            reestrs.Where(p => p.IsCompleted && (p.Date > from && p.Date < to));

        public static IQueryable<Reestr> CompletedByYearAndMonth(this IQueryable<Reestr> reestrs, int year, int month) =>
           reestrs.Where(p => p.Date.Year == year && p.Date.Month == month && p.IsCompleted);

        public static IQueryable<Reestr> ByReestrId(this IQueryable<Reestr> reestrs, Guid reestrId) =>
           reestrs.Where(p => p.Id == reestrId);

        public static IQueryable<Reestr> ByReestrIds(this IQueryable<Reestr> reestrs, IEnumerable<Guid> reestrIds) =>
           reestrs.Where(p => reestrIds.Contains(p.Id));
    }

}
