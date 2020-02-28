using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Entities.EGISSO
{
    public class KpCode: IBenefitsEntity
    {
      
        public Guid Id { get; set; }

        // "\d{2} \d{2} \d{2} \d{2}"
        [MaxLength(11)]
        public string Code { get; set; }

        public string Title { get; set; }

    }
}
