using System;
using System.Collections.Generic;
using System.Text;

namespace OldDataImport.App.Entities
{
    public interface IReshenie
    {
        int NUMBER { get; set; }
        string NR_RESH { get; set; }
        DateTime DT_RESH { get; set; }
        DateTime DT_ISPOLN { get; set; }
        decimal MDS { get; set; }
        decimal OKLAD { get; set; }
        decimal PENS { get; set; }
        decimal DOPLATA { get; set; }
        decimal MDS_PROC { get; set; }
        decimal PROC_DS { get; set; }
        decimal VID_RESH { get; set; }
        decimal PRICHINA { get; set; }
        bool OTMENA { get; set; }
        string NR_PORUCH { get; set; }
        DateTime DT_PORUCH { get; set; }
        decimal POV_KOEFF { get; set; }
    }
}
