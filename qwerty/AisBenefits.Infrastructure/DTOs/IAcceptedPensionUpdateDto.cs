using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.DTOs
{
    public interface IAcceptedPensionUpdateDto
    {
         DateTime Destination { get; set; }
         DateTime Execution { get; set; }
         string Comment { get; set; }
         Guid[] GosPensionUpdateIds { get; set; }
    }
}
