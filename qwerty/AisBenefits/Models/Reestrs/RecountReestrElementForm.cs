using AisBenefits.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Reestrs
{
    public class RecountReestrElementForm : IRecountReestrElementDTO
    {
        [Required]
        public Guid ReestrElementId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal NewSumm { get ; set ; }
    }
}
