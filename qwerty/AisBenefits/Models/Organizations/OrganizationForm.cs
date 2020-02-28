using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Organizations
{
    public class OrganizationForm
    {
        public Guid? Id { get; set; }
        [StringLength(256, MinimumLength = 2)]
        public string Name { get; set; }
        //[StringLength(2)]
        [Range(1.0, 50.0)]
        public decimal Multiplier { get; set; }
    }
}
