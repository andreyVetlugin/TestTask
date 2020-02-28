using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataLayer.Entities.EGISSO
{
    public class KpCodeLink: IBenefitsEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PrivilegeId { get; set; }
        public Guid KpCodeId { get; set; }

        public Guid MeasureUnitId { get; set; }
        public double Value { get; set; }

        public Guid EgissoId { get; set; }

        public bool Active { get; set; }

        [ForeignKey("PrivilegeId")]
        protected virtual Privilege Privilege { get; set; }
        [ForeignKey("CategoryId")]
        protected virtual KpCode KpCode { get; set; }
    }

    public static class KpCodeLinkQueryExtensions
    {
        public static IQueryable<KpCodeLink> Active(this IQueryable<KpCodeLink> query) =>
            query.Where(l => l.Active);
    }
}
