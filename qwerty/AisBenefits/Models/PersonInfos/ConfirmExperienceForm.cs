using AisBenefits.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.DTOs;

namespace AisBenefits.Models.PersonInfos
{
    public class ConfirmExperienceForm: IConfirmExperienceDto
    {
        [ValidGuid]
        public Guid PersonInfoId { get; set; }
        public bool Approved { get; set; }
       
        public DateTime DocsSubmitDate { get; set; }
        
        public DateTime DocsDestinationDate { get; set; }
    }
}
