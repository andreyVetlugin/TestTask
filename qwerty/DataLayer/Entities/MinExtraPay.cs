using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Entities
{
    public class MinExtraPay : IBenefitsEntity
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}
