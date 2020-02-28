using System;
using DataLayer.Entities;
using OldDataImport.App.Entities;

namespace OldDataImport.App
{
    public static class FunctionDbfMapper
    {
        public static Function Map(IDolg dolg)
        {
            return new Function
            {
                Id = Guid.NewGuid(),
                Name = dolg.DOLGN
            };
        }

        public static Function Map(IStag stag)
        {
            return new Function
            {
                Id = Guid.NewGuid(),
                Name = stag.DOLGN
            };
        }
    }
}
