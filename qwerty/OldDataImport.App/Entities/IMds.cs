using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Entities;

namespace OldDataImport.App.Entities
{
    public interface IMds
    {
        string Period { get; set; }

        decimal Mds { get; set; }
        int Years { get; set; }
        int Months { get; set; }
        string Type { get; set; }
    }
}
