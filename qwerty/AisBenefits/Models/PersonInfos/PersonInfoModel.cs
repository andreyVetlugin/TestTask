using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AisBenefits.Models.PersonInfos
{
    public class PersonInfoModel
    {
        public Guid? RootId { get; set; }
        public int Number { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string EmployeeType { get; set; }

        public string District { get; set; }

        public string PensionType { get; set; }
        public string PayoutType { get; set; }
        public DateTime? PensionEndDate { get; set; }
        public string AdditionalPension { get; set; }


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

        public string DocType { get; set; }
        public string CodeEgisso { get; set; }
        public string DocNumber { get; set; }
        public string DocSeria { get; set; }     
        public string Issuer { get; set; }
        public DateTime? IssueDate { get; set; }

        public string PensionCaseNumber { get; set; }
    }
}
