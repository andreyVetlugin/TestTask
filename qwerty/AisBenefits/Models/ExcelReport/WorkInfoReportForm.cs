using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.ExcelReport
{
    public class WorkInfoReportForm
    {
        public FilterItemString Experience { get; set; }
        public FilterItem<DateTime> DismissalDate { get; set; }
       
        public FilterItem<DateTime> DocsSubmitDate { get; set; }
        public FilterItem<DateTime> DocsDestinationDate { get; set; }

       
        public FilterItem<Guid> OrganizationId { get; set; }
        public FilterItem<Guid> FunctionId { get; set; }

        public FilterItem<DateTime> StartDate { get; set; }
        public FilterItem<DateTime> EndDate { get; set; }

        public FilterItem<bool> Approved { get; set; }


    }

    public static class WorkInfoReportFormExtensions
    {
        public static bool HasAnyFilter(this WorkInfoReportForm form) =>
            form.Experience.IsFiltered ||
            form.DismissalDate.IsFiltered ||
            form.DocsSubmitDate.IsFiltered ||
            form.OrganizationId.IsFiltered ||
            form.FunctionId.IsFiltered ||
            form.StartDate.IsFiltered ||
            form.EndDate.IsFiltered ||
            form.Approved.IsFiltered;
    }
}
