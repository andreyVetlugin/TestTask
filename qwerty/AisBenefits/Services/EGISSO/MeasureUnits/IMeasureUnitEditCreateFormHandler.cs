using AisBenefits.Models.EGISSO.MeasureUnits;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;

namespace AisBenefits.Services.EGISSO.MeasureUnits
{
    public interface IMeasureUnitEditCreateFormHandler
    {
        OperationResult Handle(MeasureUnitEditForm form, ModelStateDictionary modelState);
    }
}
