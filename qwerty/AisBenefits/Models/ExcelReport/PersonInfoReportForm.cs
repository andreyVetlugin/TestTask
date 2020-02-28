using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.ExcelReport
{
    public class PersonInfoReportForm
    {
        public FilterItem<int> Number { get; set; }
        public FilterItem<Guid> EmployeeTypeId { get; set; }
        public FilterItem<Guid> DistrictId { get; set; }
        public FilterItem<Guid> PensionTypeId { get; set; }
        public FilterItem<Guid> PayoutTypeId { get; set; }
        public FilterItem<DateTime> PensionEndDate { get; set; }
        public FilterItem<Guid> AdditionalPensionId { get; set; }
        public FilterItemString Fio { get; set; }
        public FilterItem<char> Sex { get; set; }
        public FilterItemString Birthplace { get; set; }
        public FilterItem<DateTime> BirthDate { get; set; }
        public FilterItemString SNILS { get; set; }
        public FilterItemString Phone { get; set; }
        public FilterItemString Address { get; set; }
        public FilterItemString DocNumber { get; set; }
        public FilterItemString DocSeria { get; set; }
        public FilterItemString Issuer { get; set; }
        public FilterItem<DateTime> IssueDate { get; set; }
        public FilterItemString PensionCaseNumber { get; set; }
        public FilterItem<bool> Approved { get; set; }

    }
}
