using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.App.Test.Controllers.ReestrController
{
    static class ReestrData
    {
        public static Guid PersonInfoId = Guid.NewGuid();

        public static ICollection<IBenefitsEntity> AddFirstReestrData(this ICollection<IBenefitsEntity> collection)
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
                Execution = new DateTime(2019, 3, 1),
                Type = SolutionType.Opredelit,
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
                Execution = new DateTime(2019, 4, 1),
                Type = SolutionType.Pereraschet,
                Comment = "Актуальное решение",
                TotalPension = 1000,
                TotalExtraPay = 1000,
                DS = 1000,
                DSperc = 1000
            });

            var reestrId = Guid.NewGuid();
            collection.Add(new Reestr
            {
                Id = reestrId,
                Date = new DateTime(2019, 3, 1),
                IsCompleted = true
            });
            collection.Add(new ReestrElement
            {
                Id = Guid.NewGuid(),
                ReestrId = reestrId,
                PersonInfoRootId = PersonInfoId
            });

            return collection;
        }

        public static ICollection<IBenefitsEntity> AddSecondReestrData(this ICollection<IBenefitsEntity> collection)
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
                OutdateTime = new DateTime(2019, 2, 13),
                Destination = new DateTime(2019, 1, 1),
                Execution = new DateTime(2019, 3, 20),
                Type = SolutionType.Opredelit,
                Comment = "Предыдущее решение",
                TotalPension = 500,
                TotalExtraPay = 1000,
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
                Execution = new DateTime(2019, 4, 16),
                Type = SolutionType.Pereraschet,
                Comment = "Актуальное решение",
                TotalPension = 500,
                TotalExtraPay = 2000,
                DS = 500,
                DSperc = 500
            });

            var reestrId = Guid.NewGuid();
            collection.Add(new Reestr
            {
                Id = reestrId,
                Date = new DateTime(2019, 3, 1),
                IsCompleted = true
            });
            collection.Add(new ReestrElement
            {
                Id = Guid.NewGuid(),
                ReestrId = reestrId,
                PersonInfoRootId = PersonInfoId
            });

            return collection;
        }


        public static ICollection<IBenefitsEntity> AddThirdReestrData(this ICollection<IBenefitsEntity> collection)
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
                CreateTime = new DateTime(2019, 3, 12),
                OutdateTime = null,
                Destination = new DateTime(2019, 3, 12),
                Execution = new DateTime(2019, 4, 16),
                Type = SolutionType.Opredelit,
                Comment = "Актуальное решение",
                TotalPension = 500,
                TotalExtraPay = 1000,
                DS = 500,
                DSperc = 500
            });

            return collection;
        }


        public static ICollection<IBenefitsEntity> AddFourthReestrData(this ICollection<IBenefitsEntity> collection)
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
                CreateTime = new DateTime(2019, 3, 12),
                OutdateTime = new DateTime(2019, 3, 12),
                Destination = new DateTime(2019, 3, 12),
                Execution = new DateTime(2019, 1, 1),
                Type = SolutionType.Opredelit,
                Comment = "Актуальное решение",
                TotalPension = 500,
                TotalExtraPay = 1000,
                DS = 500,
                DSperc = 500
            });

            //collection.Add(new ReestrElement
            //{
            //    Id = Guid.NewGuid(),
            //    PersonInfoRootId = PersonInfoId,
            //    Comment = "Выплата",
            //    Summ = 0
            //});

            collection.Add(new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                CreateTime = new DateTime(2019, 3, 12),
                OutdateTime = null,
                Destination = new DateTime(2019, 3, 12),
                Execution = new DateTime(2019, 3, 31),
                Type = SolutionType.Pause,
                Comment = "Актуальное решение",
                TotalPension = 500,
                TotalExtraPay = 1000,
                DS = 500,
                DSperc = 500
            });

            return collection;
        }


        public static ICollection<IBenefitsEntity> AddFivthReestrData(this ICollection<IBenefitsEntity> collection)
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
                CreateTime = new DateTime(2019, 3, 12),
                OutdateTime = new DateTime(2019, 3, 12),
                Destination = new DateTime(2019, 3, 12),
                Execution = new DateTime(2019, 1, 1),
                Type = SolutionType.Opredelit,
                Comment = "Актуальное решение",
                TotalPension = 500,
                TotalExtraPay = 2000,
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
                Execution = new DateTime(2019, 2, 1),
                Type = SolutionType.Pereraschet,
                Comment = "Актуальное решение",
                TotalPension = 500,
                TotalExtraPay = 1000,
                DS = 500,
                DSperc = 500,
                IsElite = false
            });
            collection.Add(new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                CreateTime = new DateTime(2019, 3, 12),
                OutdateTime = null,
                Destination = new DateTime(2019, 4, 1),
                Execution = new DateTime(2019, 1, 1),
                Type = SolutionType.Pereraschet,
                Comment = "Актуальное решение",
                TotalPension = 500,
                TotalExtraPay = 1000,
                DS = 500,
                DSperc = 500,
                IsElite = true
            });

            collection.Add(new RecountDebt
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                Debt = -500,
                MonthlyPay = -1000
            });

            var reestr1 = Guid.NewGuid();
            var reestr2 = Guid.NewGuid();
            var reestr3 = Guid.NewGuid();
            collection.Add(new Reestr
            {
                Id = reestr1,
                Date = new DateTime(2019,1,25),
                InitDate = new DateTime(2019, 1, 25),
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
                Summ = 2000
            });
            collection.Add(new ReestrElement
            {
                Id = Guid.NewGuid(),
                ReestrId = reestr2,
                PersonInfoRootId = PersonInfoId,
                Comment = "Выплата",
                Summ = 1000
            });
            collection.Add(new ReestrElement
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                ReestrId = reestr3,
                Comment = "Выплата",
                Summ = 1000
            });

            return collection;
        }

        public static ICollection<IBenefitsEntity> AddReadyToPayPerson(this ICollection<IBenefitsEntity> data, out PersonInfo personInfo)
        {
            personInfo = new PersonInfo
            {
                Id = PersonInfoId,
                RootId = PersonInfoId,
                CreateTime = new DateTime(2000, 1, 1),

                Sex = 'ж',
                Approved = true
            };
            data.Add(new PersonBankCard
            {
                Id = Guid.NewGuid(),
                PersonRootId = personInfo.RootId,
                Number = "12",
                Type = PersonBankCardType.Account,
            });

            data.Add(personInfo);

            return data;
        }

        public static ICollection<IBenefitsEntity> AddSolution(this ICollection<IBenefitsEntity> data,
            out Solution solution, DateTime date, bool actual, SolutionType type, decimal pay)
        {
            solution = new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                CreateTime = date,
                OutdateTime = actual ? (DateTime?)null : new DateTime(),
                Destination = date,
                Execution = date,
                Type = type,
                TotalPension = 500,
                TotalExtraPay = pay,
                DS = 500,
                DSperc = 500,
                IsElite = false
            };

            data.Add(solution);
            return data;
        }

        public static ICollection<IBenefitsEntity> AddReestr(this ICollection<IBenefitsEntity> data,
            out Reestr reestr, DateTime date, decimal pay)
        {
            reestr = new Reestr
            {
                Date = date,
                Id = Guid.NewGuid(),
                IsCompleted = true
            };

            data.Add(new ReestrElement
            {
                ReestrId = reestr.Id,
                
                Summ = pay
            });

            data.Add(reestr);
            return data;
        }
    }
}
