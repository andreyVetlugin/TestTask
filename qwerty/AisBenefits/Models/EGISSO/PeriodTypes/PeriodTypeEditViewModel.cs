using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.EGISSO.PeriodTypes
{
    public class PeriodTypeEditViewModel
    {
        public PeriodTypeEditForm Form { get; private set; }

        public PeriodTypeEditType Type { get; private set; }

        public string Error { get; private set; }

        public PeriodTypeEditViewModel(PeriodTypeEditForm form, PeriodTypeEditType type, string error)
        {
            Form = form;
            Type = type;
            Error = error;
        }
    }

    public enum PeriodTypeEditType
    {
        Create,
        Edit
    }
}
