using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.DTOs
{
    public interface ISolutionForm
    {
         Guid PersonInfoRootId { get; set; }

         DateTime Destination { get; set; }
         DateTime Execution { get; set; }

         string Comment { get; set; }
    }
}
