using System;
using DataLayer.Entities;

namespace AisBenefits.Infrastructure.DTOs
{
    public interface IPersonBankCardDto
    {
        Guid PersonRootId { get; set; }

        PersonBankCardType Type { get; set; }

        string Number { get; set; }

        DateTime? ValidThru { get; set; }
    }
}
