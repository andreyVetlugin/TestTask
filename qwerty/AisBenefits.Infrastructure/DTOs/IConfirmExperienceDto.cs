using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.DTOs
{
    public interface IConfirmExperienceDto
    {
         Guid PersonInfoId { get; set; }

         bool Approved { get; set; }

         DateTime DocsSubmitDate { get; set; }

         DateTime DocsDestinationDate { get; set; }
    }
}
