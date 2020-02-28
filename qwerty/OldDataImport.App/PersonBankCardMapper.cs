using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Entities;
using OldDataImport.App.Entities;

namespace OldDataImport.App
{
    public static class PersonBankCardMapper
    {
        public static PersonBankCard Map(PersonInfo personInfo, IReestr reestr)
        {
            return new PersonBankCard
            {
                Id = Guid.NewGuid(),
                PersonRootId = personInfo.RootId,
                Type = PersonBankCardType.Account,
                Number = reestr.NCARD,
                CreateDate = reestr.DATA,
                OutDate = null,
            };
        }
    }
}
