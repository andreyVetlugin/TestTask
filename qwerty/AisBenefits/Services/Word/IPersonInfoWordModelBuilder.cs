using Microsoft.AspNetCore.Mvc;
using AisBenefits.Models.PersonInfos;

namespace AisBenefits.Services.Word
{
    public interface IPersonInfoWordModelBuilder
    {
        FileResult BuildTotal(PersonInfoModel personInfoModel, WorkInfoModel workInfoModel);
    }
}
