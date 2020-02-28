using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Entities.EGISSO
{
    public class EgissoFact: IBenefitsEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public Guid PersonInfoRootId { get; set; }
        public Guid PersonInfoId { get; set; }
        public Guid SolutionId { get; set; }

        public DateTime DecisionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid PrivilegeId { get; set; }
        public Guid CategoryLinkId { get; set; }
        public Guid ProvisionFormId { get; set; }
        public Guid MeasureUnitId { get; set; }
    }
}
