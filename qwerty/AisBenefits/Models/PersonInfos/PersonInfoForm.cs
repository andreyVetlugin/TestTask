using AisBenefits.Attributes.Validation;
using AisBenefits.Infrastructure.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace AisBenefits.Models.PersonInfos
{
    public class PersonInfoForm : IPersonInfoDTO
    {
        public Guid? RootId { get; set; }

        public int Number { get; set; }
        [RegularExpression(@"^\d+$")]
        public string PensionCaseNumber { get; set; }

        public Guid EmployeeTypeId { get; set; }
        
        public Guid DistrictId { get; set; }

        public Guid PensionTypeId { get; set; }
        public Guid PayoutTypeId { get; set; }
        public DateTime? PensionEndDate { get; set; }
        public Guid AdditionalPensionId { get; set; }
        

        [Required(ErrorMessage ="Введите имя")]
        public string Name { get; set; }

        
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Введите Фамилию")]
        public string SurName { get; set; }
            
        public string Birthplace { get; set; }

        [Required(ErrorMessage = "Введите дату рождения")]
        [DataType(DataType.Date)]
        [MyDate(ErrorMessage ="Введите настоящую дату")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Выберите пол")]
        [RegularExpression("М|Ж", ErrorMessage ="М/Ж")]
        public char Sex { get; set; } //!!!

        
        //ХХХ-ХХХ-ХХХ ХХ
        public string SNILS { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
        
        public string DocTypeId { get; set; }

        public string DocNumber { get; set; }
        
        public string DocSeria { get; set; }
        
        public string Issuer { get; set; }
        
        public DateTime? IssueDate { get; set; }

        public Guid? VariantId { get; set; }
    }
}
