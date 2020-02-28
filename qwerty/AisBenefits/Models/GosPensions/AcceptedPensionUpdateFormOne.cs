using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.DTOs;

namespace AisBenefits.Models.GosPensions
{
    public class AcceptedPensionUpdateFormOne: IAcceptedPensionUpdateDtoOne
    {
        public DateTime Destination { get; set; }
        public DateTime Execution { get; set; }
        public string Comment { get; set; }
        public Guid GosPensionUpdateId { get; set; }
    }
}
