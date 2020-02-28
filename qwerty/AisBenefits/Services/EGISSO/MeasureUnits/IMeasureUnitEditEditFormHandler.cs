using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.MeasureUnits;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.MeasureUnits
{
    public interface IMeasureUnitEditEditFormHandler
    {
        OperationResult Handle(MeasureUnitEditForm form, ModelStateDictionary modelState);
    }
}
