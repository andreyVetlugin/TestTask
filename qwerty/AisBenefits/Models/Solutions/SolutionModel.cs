using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Solutions
{
    public class SolutionModel
    {
        public Guid Id { get; set; }

        public DateTime Destination { get; set; }
        public DateTime Execution { get; set; }

        public string Comment { get; set; }
        public string SolutionType_str { get; set; }

        public string TotalPension { get; set; }
        public string TotalExtraPay { get; set; }
        public string DS { get; set; }
        public string Mds { get; set; }
        public decimal DSperc { get; set; }

        public bool AllowDelete { get; set; }
    }
}
