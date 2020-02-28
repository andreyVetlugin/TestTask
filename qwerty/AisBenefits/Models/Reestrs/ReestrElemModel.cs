using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Reestrs
{
    public class ReestrElemModel
    {
        public Guid Id { get; set; }
        public int PersonInfoNumber { get; set; }
        public string SurName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string FIO => string.Join(" ", SurName, FirstName, MiddleName);
        public string Account { get; set; }
        public decimal BaseSumm { get; set; }
        public decimal Summ { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public bool Valid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
