using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.EGISSO.Privileges
{
    public class PrivilegeEditForm
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Введите {0}"), Display(Name = "Наименование")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Введите {0}"), RegularExpression(@"\d{4}", ErrorMessage = "Поле не соответствует формату"), Display(Name = "Код в классификаторе ЕГИССО")]
        public string EgissoCode { get; set; }

        [Display(Name = "Коды получателя (КП)")]
        public IList<PrivilegeCategoryEditForm> Categories { get; set; }

        [Display(Name = "Переодичность предоставления")]
        public Guid PeriodTypeId { get; set; }
        [Display(Name = "Форма предоставления")]
        public Guid ProvisionFormId { get; set; }
        [Required(ErrorMessage = "Выберите {0}"), Display(Name = "Признак использования критериев нуждаемости при назначении МСЗ")]
        public bool UsingNeedCriteria { get; set; }
        [Required(ErrorMessage = "Выберите {0}"), Display(Name = "Признак монетизации")]
        public bool Monetization { get; set; }

        [Display(Name = "Идентификатор ЕГИССО")]
        public Guid EgissoId { get; set; }
    }


    public class PrivilegeCategoryEditForm
    {
        public Guid CategoryId { get; set; }
        public Guid MeasureUnitId { get; set; }
        public double Value { get; set; }

        public Guid EgissoId { get; set; }

        public bool HasChanges(Guid measureUnitId, double value, Guid egissoId)
        {
            return MeasureUnitId != measureUnitId || Value != value || (EgissoId != egissoId);
        }
    }




}
