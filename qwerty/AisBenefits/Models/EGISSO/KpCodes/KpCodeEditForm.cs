using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.EGISSO.KpCodes
{
    public class KpCodeEditForm
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Введите {0}"), RegularExpression(@"\d{2} \d{2} \d{2} \d{2}", ErrorMessage = "Поле не соответствует формату"), Display(Name = "Код получателя(КП)")]
        public string KpCode { get; set; }

        [Required(ErrorMessage = "Введите {0}"), Display(Name = "Наименование")]
        public string Title { get; set; }
       
    }
}
