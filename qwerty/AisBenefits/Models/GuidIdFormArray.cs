using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models
{
    public class GuidIdFormArray
    {
        [Required]
        public Guid[] Ids { get; set; }
    }
}
