﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.KpCodes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.KpCodes
{
    public interface IKpCodeEditEditFormHandler
    {
        OperationResult Handle(KpCodeEditForm form, ModelStateDictionary modelState);

    }
}