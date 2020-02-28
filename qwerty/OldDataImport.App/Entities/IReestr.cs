using System;

namespace OldDataImport.App.Entities
{
    public interface IReestr
    {
        int NUMBER { get; set; }
        int NREESTR { get; set; }
        DateTime DATA { get; set; }
        decimal PORNOM { get; set; }
        string FIO { get; set; }
        decimal MES { get; set; }
        decimal GOD { get; set; }
        string TIP { get; set; }
        decimal SUMMA { get; set; }
        string NCARD { get; set; }
        DateTime SDT { get; set; }
        DateTime FDT { get; set; }
        string NUM_REESTR { get; set; }
        string CITIZEN { get; set; }
        bool LMONTH { get; set; }
    }
}
