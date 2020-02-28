using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.ExcelReport
{
    public class ExtraPayReportForm
    {

        public FilterItem<Guid> VariantId { get; set; }

        public FilterItem<decimal> UralMultiplier { get; set; }

        public FilterItem<decimal> Salary { get; set; }
        public FilterItem<decimal> SalaryMultiplied { get; set; }
 
        public FilterItem<decimal> Premium { get; set; }

        public FilterItem<decimal> MaterialSupport { get; set; }
        public FilterItem<decimal> MaterialSupportDivPerc { get; set; }

        public FilterItem<decimal> Perks { get; set; }
        public FilterItem<decimal> PerksDivPerc { get; set; }

        public FilterItem<decimal> Vysluga { get; set; }
        public FilterItem<decimal> VyslugaDivPerc { get; set; }

        public FilterItem<decimal> Secrecy { get; set; }
        public FilterItem<decimal> SecrecyDivPerc { get; set; }

        public FilterItem<decimal> Qualification { get; set; }
        public FilterItem<decimal> QualificationDivPerc { get; set; }

        public FilterItem<decimal> Ds { get; set; }
        public FilterItem<decimal> DsPerc { get; set; }

        public FilterItem<decimal> GosPension { get; set; }
        public FilterItem<decimal> ExtraPension { get; set; }
        
        public FilterItem<decimal> TotalExtraPay { get; set; }
        public FilterItem<decimal> TotalPension { get; set; }
        public FilterItem<decimal> TotalPensionAndExtraPay { get; set; }

    }
}
