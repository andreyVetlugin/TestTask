using AisBenefits.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Reestrs
{
    public class ReestrFileForm
    {
        public Guid Id { get; set; }
        public bool ForSignature { get; set; }
    }
}
