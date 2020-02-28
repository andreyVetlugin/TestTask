using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.GosPensions
{
    public class GosPensionUpdateModel
    {
        public int Number { get; set; }
        public string FIO { get; set; }
        public decimal CurrentPension { get; set; }

        public Guid GosPensionUpdateId { get; set; }
        public decimal NewPensionValue { get; set; }
    }
}
