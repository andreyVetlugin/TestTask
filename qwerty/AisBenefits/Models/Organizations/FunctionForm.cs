using System;
using System.ComponentModel.DataAnnotations;

namespace AisBenefits.Models.Organizations
{
    public class FunctionForm
    {
        public Guid? Id { get; set; }
        [StringLength(256, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
