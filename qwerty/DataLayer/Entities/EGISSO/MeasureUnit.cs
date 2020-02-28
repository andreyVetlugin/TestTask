using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Entities.EGISSO
{
    public class MeasureUnit : IBenefitsEntity
    {
        public Guid Id { get; set; }

        public int PpNumber { get; set; }

        // "\d{2}"
        [MaxLength(2)]
        public string PositionCode { get; set; }
        [MaxLength(32)]
        public string Title { get; set; }
        [MaxLength(16)]
        public string ShortTitle { get; set; }

        public bool IsDecimal { get; set; }

        // "\d{3}"
        [MaxLength(4)]
        public string OkeiCode { get; set; }
    }
}
