using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.DTOs;

namespace AisBenefits.Models.GosPensions
{
    public class AcceptedPensionUpdateForm: IAcceptedPensionUpdateDto
    {
        public DateTime Destination { get; set; }
        public DateTime Execution { get; set; }
        public string Comment { get; set; }
        public Guid[] GosPensionUpdateIds { get; set; }
        
    }
}
