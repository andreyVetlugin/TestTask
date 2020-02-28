using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace AisBenefits.Models.ExcelReport
{
    public class SolutionReportForm
    {
        public FilterItem<DateTime> Destination { get; set; }
        public FilterItem<DateTime> Execution { get; set; }
        public FilterItem<SolutionType> Type { get; set; }
    }
}
