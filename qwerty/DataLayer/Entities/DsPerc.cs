using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataLayer.Entities
{
    public class DsPerc : IBenefitsEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Period { get; set; }

        public decimal Amount { get; set; }

        public int AgeDays { get; set; }

        public DsPercGenderType GenderType { get; set; }
    }

    public enum DsPercGenderType
    {
        General,
        Male,
        Female
    }

    public static class DsPercQueryableExtensions
    {
        public static IQueryable<DsPerc> ById(this IQueryable<DsPerc> dsPercs, Guid dsPercId) =>
            dsPercs.Where(p => p.Id == dsPercId);

        public static IQueryable<DsPerc> ByDate(this IQueryable<DsPerc> dsPercs, DateTime date) =>
            dsPercs.Where(p => p.Period <= date && p.Period.Year == date.Year);

        public static IQueryable<DsPerc> ByDateAndAge(this IQueryable<DsPerc> dsPercs, DateTime date, int ageDays) =>
            dsPercs.Where(p => p.Period <= date && p.Period.Year == date.Year && p.AgeDays <= ageDays).OrderByDescending(p => p.AgeDays).Take(1);
    }
}
