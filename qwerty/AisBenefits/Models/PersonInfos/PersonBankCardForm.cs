using AisBenefits.Infrastructure.DTOs;
using System;
using DataLayer.Entities;

namespace AisBenefits.Models.PersonInfos
{
    public class PersonBankCardForm : IPersonBankCardDto
    {      

        public Guid PersonRootId { get; set; }

        public PersonBankCardType Type { get; set; }

        public string Number { get; set; }

        public DateTime? ValidThru { get; set; }
    }
}
