using AisBenefits.Infrastructure.Services.ExtraPays;
using System;
using System.ComponentModel.DataAnnotations;

namespace AisBenefits.Models.ExtraPays
{
    public class ExtraPayEditForm : IExtraPayEditForm
    {
        public Guid PersonRootId { get; set; }

        public Guid VariantId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UralMultiplier { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Premium { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MaterialSupport { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Perks { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Vysluga { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Secrecy { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Qualification { get; set; }

        [Range(0, double.MaxValue)]
        public decimal GosPension { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ExtraPension { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Ds { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalExtraPay { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalPension { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalPensionAndExtraPay { get; set; }

        public DateTime DestinationDate { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string Comment { get; set; }

        public bool CreateSolution { get; set; }
    }
}
