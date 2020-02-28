using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AisBenefits.Infrastructure.DTOs
{
    public interface IPersonInfoDTO
    {
        Guid? RootId { get; set; }
        int Number { get; set; }

        Guid EmployeeTypeId { get; set; }

        Guid DistrictId { get; set; }

        Guid PensionTypeId { get; set; }
        Guid PayoutTypeId { get; set; }
        DateTime? PensionEndDate { get; set; }
        Guid AdditionalPensionId { get; set; }


        string Name { get; set; }
        string MiddleName { get; set; }
        string SurName { get; set; }
        string Birthplace { get; set; }
        DateTime BirthDate { get; set; }
        char Sex { get; set; } //!!!

        string SNILS { get; set; }

        string Phone { get; set; }
        string Email { get; set; }
        
        string Address { get; set; }

        string DocTypeId { get; set; }
        //string CodeEgisso { get; set; }
        string DocNumber { get; set; }
        string DocSeria { get; set; }


        string Issuer { get; set; }

        DateTime? IssueDate { get; set; }

        string PensionCaseNumber { get; set; }

        Guid? VariantId { get; set; }
    }
}
