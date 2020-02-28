using System;
using System.Collections.Generic;
using System.Text;

namespace OldDataImport.App.Entities
{
    public interface IDolg
    {
        string DOLGN { get; set; }
        decimal NR { get; set; }
        bool L_KORR { get; set; }
        bool L_DEL { get; set; }
        decimal MDS_GOR { get; set; }
        decimal MDS_RAI { get; set; }
        decimal OKL_GOR { get; set; }
        decimal OKL_RAI { get; set; }
    }
}
