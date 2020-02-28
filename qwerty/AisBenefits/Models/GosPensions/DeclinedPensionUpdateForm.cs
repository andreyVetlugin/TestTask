using AisBenefits.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.GosPensions
{
    public class DeclinedPensionUpdateForm : IDeclinedPensionsDTO
    {
        [Required]
        public Guid IncomePensionId { get; set; }
        //[Required]
        //public decimal EditedGosPensionValue { get; set; }
    }
}
