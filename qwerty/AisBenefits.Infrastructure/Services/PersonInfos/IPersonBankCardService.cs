using AisBenefits.Infrastructure.DTOs;
using DataLayer.Entities;
using System;

namespace AisBenefits.Infrastructure.Services.PersonInfos
{
    public interface IPersonBankCardService
    {
        OperationResult Create(IPersonBankCardDto personBankCardDto);
        OperationResult Update(IPersonBankCardDto personBankCardDto);
        ModelDataResult<PersonBankCard> Get(Guid personInfoRootId);
    }
}
