using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Reestrs
{
    public class GetReestrArchiveForm
    {
        [Required]
        [Range(0,3000)]
        public int Year { get; set; }
        [Required]
        [Range(1,12)]
        public int Month { get; set; }
    }
}
