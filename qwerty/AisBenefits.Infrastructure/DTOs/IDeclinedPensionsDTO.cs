using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.DTOs
{
    public interface IDeclinedPensionsDTO
    {
        Guid IncomePensionId { get; set; }
        //decimal EditedGosPensionValue { get; set; }
    }
}
