using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.GosPensions
{
    public class DeclinedPensionsArray
    {
        [Required]
        public DeclinedPensionUpdateForm[] DeclinedPensionUpdates {get;set;}
    }
}
