using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.EGISSO.MeasureUnits
{
    public class MeasureUnitEditForm
    {
        public Guid? Id { get; set; }
        [Display(Name = "№ п/п"), Required(ErrorMessage = "Введите {0}")]
        public int PpNumber { get; set; }
        [Display(Name = "Код позиции"), RegularExpression(@"\d{2}", ErrorMessage = "Поле не соответсвует формату"), Required()]
        public string PositionCode { get; set; }
        [Display(Name = "Наименование"), Required(ErrorMessage = "Введите {0}")]
        public string Title { get; set; }
        [Display(Name = "Краткое наименование"), Required(ErrorMessage = "Введите {0}")]
        public string ShortTitle { get; set; }
        [Display(Name = "Код ОКЕИ"), RegularExpression(@"\d{3}", ErrorMessage = "Поле не соответсвует формату")]
        public string OkeiCode { get; set; }
        [Display(Name = "Представление")]
        public bool IsDecimal { get; set; }
    }
}
