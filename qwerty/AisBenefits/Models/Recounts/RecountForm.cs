using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Recounts
{
    public class RecountForm
    {
        public Guid PersonInfoRootId { get; set; }
        public DateTime Date { get; set; }
        public decimal GosPension { get; set; }
        public decimal ExtraPension { get; set; }
        public decimal Summ { get; set; }
    }
}
