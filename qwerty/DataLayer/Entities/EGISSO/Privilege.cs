using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Entities.EGISSO
{
    public class Privilege : IBenefitsEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Guid PeriodTypeId { get; set; }
        public Guid ProvisionFormId { get; set; }

        public bool UsingNeedCriteria { get; set; }

        public bool Monetization { get; set; }

        // "\d{4}"
        [MaxLength(4)]
        public string EgissoCode { get; set; }

        public Guid EgissoId { get; set; }

        [ForeignKey("PeriodTypeId")]
        protected virtual PeriodType PeriodType { get; set; }
    }
}
