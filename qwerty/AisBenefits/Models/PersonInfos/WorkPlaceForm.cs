using AisBenefits.Attributes.Validation;
using AisBenefits.Infrastructure.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace AisBenefits.Models.PersonInfos
{
    public class WorkPlaceForm:IWorkInfoDTO
    {
        public Guid? RootId { get; set; }

        [Required(ErrorMessage = "Нужно указать дату")]
        [MyDate(ErrorMessage = "Укажите корректную дату")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Нужно указать дату")]
        [MyDate(ErrorMessage = "Укажите корректную дату")]
        public DateTime EndDate { get; set; }
        
        public Guid OrganizationId { get; set; }
        public Guid FunctionId { get; set; }

        [Required(ErrorMessage = "Не указано название компании")]
        public string Organization { get; set; }
        [Required(ErrorMessage = "Ваша должность")]
        public string Function { get; set; }
    }
}
