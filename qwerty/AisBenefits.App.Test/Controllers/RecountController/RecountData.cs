using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.App.Test.Controllers.RecountController
{
    public static class RecountData
    {
        public static Guid PersonInfoId = Guid.NewGuid();

        public static ICollection<IBenefitsEntity> AddFirstRecountData(this ICollection<IBenefitsEntity> collection)
        {
            collection.Add(new PersonInfo
            {
                Id = PersonInfoId,
                RootId = PersonInfoId,
                CreateTime = new DateTime(2000, 1, 1),

                Sex = 'ж',
                Approved = true
            });

            collection.Add(new PersonBankCard
            {
                Id = Guid.NewGuid(),
                PersonRootId = PersonInfoId,
                Number = "12",
                Type = PersonBankCardType.Account,
            });

            collection.Add(new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                CreateTime = new DateTime(2000, 1, 1),
                OutdateTime = new DateTime(2019, 3, 12),
                Destination = new DateTime(2019, 1, 1),
                Execution = new DateTime(2018, 11, 1),
                Type = SolutionType.Pause,
                Comment = "Предыдущее решение",
                TotalPension = 500,
                TotalExtraPay = 500,
                DS = 500,
                DSperc = 500
            });


            collection.Add(new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                CreateTime = new DateTime(2019, 3, 12),
                OutdateTime = null,
                Destination = new DateTime(2019, 3, 12),
                Execution = new DateTime(2018, 11, 7),
                Type = SolutionType.Pereraschet,
                Comment = "Актуальное решение",
                TotalPension = 1000,
                TotalExtraPay = 1000,
                DS = 1000,
                DSperc = 1000
            });
            collection.Add(new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                CreateTime = new DateTime(2019, 3, 12),
                OutdateTime = null,
                Destination = new DateTime(2019, 3, 12),
                Execution = new DateTime(2019, 1, 13),
                Type = SolutionType.Pereraschet,
                Comment = "Актуальное решение",
                TotalPension = 1000,
                TotalExtraPay = 1000,
                DS = 1000,
                DSperc = 1000
            });
            collection.Add(new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                CreateTime = new DateTime(2019, 3, 12),
                OutdateTime = null,
                Destination = new DateTime(2019, 3, 12),
                Execution = new DateTime(2018, 11, 25),
                Type = SolutionType.Pereraschet,
                Comment = "Актуальное решение",
                TotalPension = 1000,
                TotalExtraPay = 2000,
                DS = 1000,
                DSperc = 1000
            });


            var reestr1 = Guid.NewGuid();
            var reestr2 = Guid.NewGuid();
            var reestr3 = Guid.NewGuid();
            collection.Add(new Reestr
            {
                Id = reestr1,
                Date = new DateTime(2018, 11, 25),
                InitDate = new DateTime(2018, 11, 25),
                IsCompleted = true
            });
            collection.Add(new Reestr
            {
                Id = reestr2,
                Date = new DateTime(2019, 2, 25),
                InitDate = new DateTime(2019, 2, 25),
                IsCompleted = true
            });
            collection.Add(new Reestr
            {
                Id = reestr3,
                Date = new DateTime(2019, 3, 25),
                InitDate = new DateTime(2019, 3, 25),
                IsCompleted = true
            });




            collection.Add(new ReestrElement
            {
                Id = Guid.NewGuid(),
                ReestrId = reestr1,
                PersonInfoRootId = PersonInfoId,
                Comment = "Выплата",
                Summ = 4000
            });
            collection.Add(new ReestrElement
            {
                Id = Guid.NewGuid(),
                ReestrId = reestr2,
                PersonInfoRootId = PersonInfoId,
                Comment = "Выплата",
                Summ = 2000
            });
            collection.Add(new ReestrElement
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                ReestrId = reestr3,
                Comment = "Выплата",
                Summ = 1000
            });

            collection.Add(new ExtraPay
            {
                Id = Guid.NewGuid(),
                PersonRootId = PersonInfoId,
                Salary = 1000,
                UralMultiplier = 1.15m,
                TotalExtraPay = 3400
            });

            return collection;
        }
    }
}
