using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataLayer.Entities
{
    public class PfrSnilsRequest: IBenefitsEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PersonInfoRootId { get; set; }

        public DateTime Date { get; set; }
        public string Snils { get; set; }

        public PfrSnilsRequestState State { get; set; }
    }

    public enum PfrSnilsRequestState
    {
        Sended,
        Success,
        Error
    }

    public static class PfrSnilsRequestQueryExtensions
    {
        public static IQueryable<PfrSnilsRequest> Sended(this IQueryable<PfrSnilsRequest> query) =>
            query.Where(r => r.State == PfrSnilsRequestState.Sended);
    }
}
