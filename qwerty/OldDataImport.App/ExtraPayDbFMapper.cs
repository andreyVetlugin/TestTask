using System;
using DataLayer.Entities;
using OldDataImport.App.Entities;

namespace OldDataImport.App
{
    public static class ExtraPayDbFMapper
    {
        public static ExtraPay Map(PersonInfo personInfo, IReshenie reshenie, IDoplata doplata)
        {
            return new ExtraPay
            {
                Id = Guid.NewGuid(),
                
                PersonRootId = personInfo.RootId,

                CreateDate = DateTime.Now,
                OutDate = null,

                VariantId = Guid.Empty,

                Salary = doplata.OKLAD,
                GosPension = doplata.GOSPENS,
                MaterialSupport = doplata.MATPOM,
                Premium = doplata.PREMIJA,
                Qualification = doplata.KVALIF,
                Secrecy = doplata.SECRET,
                Vysluga = doplata.VYSLUGA,
                UralMultiplier = doplata.UR_KOEF,
                Perks = doplata.NADBAV,
                TotalExtraPay = doplata.DOPLATA,
                TotalPension = reshenie.PENS,
                Ds = reshenie.MDS,
                DsPerc = reshenie.PROC_DS,
                ExtraPension = doplata.DOPPENS,
                TotalPensionAndExtraPay = reshenie.MDS_PROC
            };
        }
    }
}
