using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Containers
{
    public class ExtraPayContainer
    {      

        public decimal UralMultiplier {get; set; } 
        public decimal SalaryMultiplied {get; set; } 
        public decimal PremiumPerc {get; set; } 
        public decimal Premium {get; set; }
        public decimal MatSupportMultiplier {get; set; } 
        public decimal MaterialSupport {get; set; }
        public decimal VyslugaMultiplier {get; set; }
        public decimal Vysluga {get; set; }
        public decimal VyslugaDivPerc {get; set; }        
        public decimal Ds {get; set; }
        public decimal WorkAge {get; set; } 
        public decimal DsPerc {get; set; } 
        public decimal TotalPension {get; set; } 
        public decimal TotalPensionAnExtraPay {get; set; } 
        public decimal TotalExtraPay {get; set; } 
    }
}
