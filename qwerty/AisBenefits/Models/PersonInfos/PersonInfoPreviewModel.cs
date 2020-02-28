using System;

namespace AisBenefits.Models.PersonInfos
{
    public class PersonInfoPreviewModel
    {
        public Guid RootId { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string SNILS { get; set; }
        public DateTime BirthDate { get; set; }
        public int Number { get; set; }

        public string PayoutType { get; set; }
        public string EmployType { get; set; }
        public string ExtraPayVariant { get; set; }

        public bool Approved { get; set; }
        public bool Paused { get; set; }
    }
}
