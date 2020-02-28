using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Reestrs
{
    public class ReestrOUTPUT
    {
        public ReestrModel Reestr {get;set;}
        public ReestrElemModel[] ReestrElements { get; set; }
        public int NumberOfElements { get; set; }
        public decimal SummTotal { get; set; }

        public bool CanComplete { get; set; }
    }
}
