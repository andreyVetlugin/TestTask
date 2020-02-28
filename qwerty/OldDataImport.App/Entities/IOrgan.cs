using System;
using System.Collections.Generic;
using System.Text;

namespace OldDataImport.App.Entities
{
    public interface IOrgan
    {
        string ORGAN { get; set; }
        decimal KOEFF { get; set; }
        decimal NR { get; set; }
        bool L_KORR { get; set; }
        bool L_DEL { get; set; }
        string TERRITOR { get; set; }
    }
}
