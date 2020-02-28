
using AisBenefits.Models.EGISSO.PeriodTypes;
using AisBenefits.Services.EGISSO.PeriodTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Services.EGISSO.PeriodTypes
{
    public interface IPeriodTypeEditViewModelBuilder
    {
        PeriodTypeEditViewModel BuildCreate(PeriodTypeEditForm form = null, PeriodTypeEditFormHandlerResult result = null);
        PeriodTypeEditViewModel BuildEdit(Guid periodTypeId, PeriodTypeEditForm form = null, PeriodTypeEditFormHandlerResult result = null);
    }
}
