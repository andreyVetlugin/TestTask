using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.ActionResults
{
    public class ApiForbiddenResult: ApiResult
    {

        public ApiForbiddenResult()
            : base(new { Message = "Недостаточно прав увы" }, 403)
        {
        }

    }
}
