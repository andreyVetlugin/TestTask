using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.ProvisionForms;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.ProvisionForms
{
    public interface IProvisionFormEditEditFormHandler
    {
        OperationResult Handle(ProvisionFormEditForm form, ModelStateDictionary modelState);

    }
}
