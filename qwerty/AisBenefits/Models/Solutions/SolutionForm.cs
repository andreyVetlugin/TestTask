using AisBenefits.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Solutions
{
    public class SolutionForm: ISolutionForm
    {
        public Guid PersonInfoRootId { get; set; }

        public DateTime Destination { get; set; }
        public DateTime Execution { get; set; }

        public string Comment { get; set; }
    }
}
