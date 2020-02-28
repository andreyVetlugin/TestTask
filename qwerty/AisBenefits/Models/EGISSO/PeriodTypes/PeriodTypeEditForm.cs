using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.EGISSO.PeriodTypes
{
    public class PeriodTypeEditForm
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Введите {0}")]
        public int PpNumber { get; set; }
        [Required(ErrorMessage = "Введите {0}"), RegularExpression(@"\d{2}", ErrorMessage = "Поле не соответсвует формату")]
        public string PositionCode { get; set; }
        [Required(ErrorMessage = "Введите {0}")]
        public string Value { get; set; }
    }
}
