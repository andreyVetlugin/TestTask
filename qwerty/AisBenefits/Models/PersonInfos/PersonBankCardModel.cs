using System;
using DataLayer.Entities;

namespace AisBenefits.Models.PersonInfos
{
    public class PersonBankCardModel
    {
        public Guid Id { get; set; }

        public Guid PersonRootId { get; set; }

        public PersonBankCardType Type { get; set; }
        public string Number { get; set; }
        public DateTime? ValidThru { get; set; }
    }
}
