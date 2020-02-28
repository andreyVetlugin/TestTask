using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Models.PersonInfos;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Services.Word
{
    public interface IExtraPayWordModelBuilder
    {
        FileResult BuildTotal(Guid personRootId, DateTime from, DateTime to);
    }
}
