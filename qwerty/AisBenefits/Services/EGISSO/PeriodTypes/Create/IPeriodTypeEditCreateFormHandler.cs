using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.PeriodTypes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Services.EGISSO.PeriodTypes.Create
{
    public interface IPeriodTypeEditCreateFormHandler
    {
        OperationResult Handle(PeriodTypeEditForm form, ModelStateDictionary modelState);
    }
}
