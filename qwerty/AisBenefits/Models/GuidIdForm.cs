using AisBenefits.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models
{
    public class GuidIdForm
    {
        [ValidGuid]
        public Guid Id { get; set; }
    }
}
