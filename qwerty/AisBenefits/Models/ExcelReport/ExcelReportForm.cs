using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.ExcelReport
{
    public class ExcelReportForm
    {
        public PersonInfoReportForm PIForm { get; set; }
        public WorkInfoReportForm WIForm { get; set; }
        public ExtraPayReportForm EPForm { get; set; }
        public PayoutReportForm PayoutForm { get; set; }
        public SolutionReportForm SolForm { get; set; }
    }
}
