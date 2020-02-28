using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DataLayer.Entities
{
    public class ZagsStopAct: IBenefitsEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Number { get; set; }

        public DateTime Date { get; set; }

        public Guid PersonInfoRooId { get; set; }

        public ZagsStopActState State { get; set; }
    }

    public enum ZagsStopActState
    {
        New = 0,

        Approved = 10,

        Declined = 20
    }

    public static class ZagsStopActQueryExtensions
    {
        public static IQueryable<ZagsStopAct> ById(this IQueryable<ZagsStopAct> query, Guid id) =>
            query.Where(a => a.Id == id);
        public static IQueryable<ZagsStopAct> New(this IQueryable<ZagsStopAct> query) =>
            query.Where(a => a.State == ZagsStopActState.New);

        public static IQueryable<ZagsStopAct> ByNumbers(this IQueryable<ZagsStopAct> query, IEnumerable<string> numbers) =>
            query.Where(a => numbers.Contains(a.Number));

        public static IQueryable<ZagsStopAct> ByPersonInfoRootIds(this IQueryable<ZagsStopAct> query, IEnumerable<Guid> rootIds) =>
            query.Where(a => rootIds.Contains(a.PersonInfoRooId));
    }
}
