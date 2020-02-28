using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.Services.PersonInfos
{

    public interface IEnumPersonInfoService
    {
        PersonInfo ToEnums(PersonInfo personInfo);
        PersonInfo ToStrings(PersonInfo personInfo);
    }

    class EnumPersonInfoService
    {
    }
}
