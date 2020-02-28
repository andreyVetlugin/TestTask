using System;

namespace OldDataImport.App.Entities
{
    public interface IStag
    {
        int NUMBER { get; set; }
        string CODE { get; set; }
        string NPR { get; set; }
        DateTime DATA_BEGIN { get; set; }
        DateTime DATA_END { get; set; }
        decimal KODUCHREG { get; set; }
        string UCHREGD { get; set; }
        decimal KODDOLGN { get; set; }
        string DOLGN { get; set; }
        decimal KOEF { get; set; }
        string STAG_GOD { get; set; }
        string STAG_MES { get; set; }
        string STAG_DNI { get; set; }
    }
}
