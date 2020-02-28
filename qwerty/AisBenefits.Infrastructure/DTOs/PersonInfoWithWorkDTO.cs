using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.DTOs
{
    public class PersonInfoWithWorkDTO
    {
        public Guid Id { get; set; }
        public Guid? NextId { get; set; }
        public Guid RootId { get; set; }

        
        public int Number { get; set; }

        public Guid EmployeeTypeId { get; set; }

        public Guid DistrictId { get; set; }

        public Guid PensionTypeId { get; set; }
        public DateTime? PensionEndDate { get; set; }
        public Guid AdditionalPensionId { get; set; }


        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string Birthplace { get; set; }
        public DateTime BirthDate { get; set; }
        public char Sex { get; set; } //!!!

        public string SNILS { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }

        public string DocTypeId { get; set; }
        public string CodeEgisso { get; set; }
        public string DocNumber { get; set; }
        public string DocSeria { get; set; }
        public string Issuer { get; set; }
        public DateTime IssueDate { get; set; }

        public bool Approved { get; set; }
        public DateTime? DocsSubmitDate { get; set; }

    }
}
