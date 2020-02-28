using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Services.EGISSO.PeriodTypes
{
    public class PeriodTypeEditFormHandlerResult
    {
        public bool Ok { get { return Error == null; } }

        public string Error { get; private set; }

        public PeriodTypeEditFormHandlerResult(string error)
        {
            Error = error;
        }
    }
}
