using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Entities;
using OldDataImport.App.Entities;

namespace OldDataImport.App
{
    public static class ReestrDbfMapper
    {
        public static List<Reestr> Map(IEnumerable<IReestr> reestr)
        {
            return reestr
                .GroupBy(r => r.NREESTR)
                .Select(g =>
                {
                    var e = g.First();
                    return new Reestr
                    {
                        Id = Guid.NewGuid(),
                        Date = e.DATA,
                        InitDate = e.DATA,
                        Number = e.NREESTR,
                        IsCompleted = true
                    };
                })
                .ToList();
        }

        public static ReestrElement Map(Reestr reestr, PersonInfo personInfo, PersonBankCard card, IReestr element)
        {
            return new ReestrElement
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = personInfo.RootId,
                PersonInfoId = personInfo.Id,
                ReestrId = reestr.Id,
                Deleted = false,
                BaseSumm = element.SUMMA,
                Summ = element.SUMMA,
                PersonBankCardId = card.Id
            };
        }
    }
}
