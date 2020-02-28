using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.Reestrs;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Services.Reestrs
{
    public class ReestrElemModelBuilder : IReestrElemModelBuilder
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public ReestrElemModelBuilder(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public ReestrElemModel[] Build(List<ReestrElement> reestrElements)
        {
            var res = new List<ReestrElemModel>();

            var personInfos = readDbContext.Get<PersonInfo>()
                .ByCurrentIds(reestrElements.Select(c => c.PersonInfoId))
                .ToDictionary(p => p.Id);

            var cards = readDbContext.Get<PersonBankCard>()
                .ActualByPersonInfoRootIds(reestrElements.Select(e => e.PersonInfoRootId))
                .ToDictionary(c => c.PersonRootId);

            foreach (var elem in reestrElements)
            {
                var personInfo = personInfos[elem.PersonInfoId];
                var card = cards.GetValueOrDefault(elem.PersonInfoRootId);

                var temp = Mapper.Map<ReestrElement, ReestrElemModel>(elem);

                temp.SurName = personInfo.SurName?.Trim() ?? string.Empty;
                temp.FirstName = personInfo.Name?.Trim() ?? string.Empty;
                temp.MiddleName = personInfo.MiddleName?.Trim() ?? string.Empty;
                
                temp.Account = card?.Number;
                temp.PersonInfoNumber = personInfo.Number;

                temp.Valid = false;
                if (card == null)
                {
                    temp.ErrorMessage = "Не указан банковский счет";
                }
                else
                {
                    temp.Valid = true;
                }

                res.Add(temp);
            }

            return res.OrderBy(e => e.FIO).ToArray();
        }
    }
}
