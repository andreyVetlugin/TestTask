using AisBenefits.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Reestrs
{
    public class ReestrForm : IReestrDTO
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime InitDate { get ; set; }
    }
}
