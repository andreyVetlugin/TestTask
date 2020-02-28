using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPOI.OpenXmlFormats.Spreadsheet;

namespace AisBenefits.Models.ExcelReport
{


    public class FilterItem<T>
    where T: struct
    {
        public bool IsShow { get; set; }
        public bool IsFiltered { get; set; }
        public T? Value { get; set; }
        public ReportFilterType FilterType { get; set;}
    }

    public enum ReportFilterType : byte
    {
        Equal,
        More,
        Less,
        NotEqual,
    }

    public class FilterItemString
    {
        public bool IsShow { get; set; }
        public bool IsFiltered { get; set; }
        public string Value { get; set; }
        public ReportFilterType FilterType { get; set; }
    }

   
}
