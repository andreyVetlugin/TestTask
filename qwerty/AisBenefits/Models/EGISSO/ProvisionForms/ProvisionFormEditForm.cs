using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.EGISSO.ProvisionForms
{
    public class ProvisionFormEditForm
    {
        public Guid? Id { get; set; }
        [Display(Name = "Наименование"), Required(ErrorMessage = "Введите {0}")]
        public string Title { get; set; }
        [Display(Name = "Код формы"), Required(ErrorMessage = "Введите {0}"), RegularExpression(@"\d{2}", ErrorMessage = "{0} не соответствует формату")]
        public string Code { get; set; }
    }
}
