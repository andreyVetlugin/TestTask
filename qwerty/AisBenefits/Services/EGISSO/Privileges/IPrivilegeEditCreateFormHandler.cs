using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.Privileges;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.Privileges
{
    public interface IPrivilegeEditCreateFormHandler
    {
        OperationResult Handle(PrivilegeEditForm form, ModelStateDictionary modelState);
    }
}
