using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Entities.EGISSO
{
    public class EgissoEjectionHistory: IBenefitsEntity
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime DestinationDate { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
