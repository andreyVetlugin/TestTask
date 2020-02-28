using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AisBenefits.Infrastructure.Services.DsPercs;

namespace AisBenefits.Models.DsPerc
{
    public class DsPercEditYearForm : IDsPercEditYearForm
    {
        public int Year { get; set; }

        [Required]
        public List<DsPercEditForm> DsPercs { get; set; }

        IReadOnlyList<IDsPercEditForm> IDsPercEditYearForm.DsPercs => DsPercs;
    }

    public class DsPercEditForm : IDsPercEditForm
    {
        [Range(10, 100)]
        public decimal Amount { get; set; }
        [Range(0, 300)]
        public int AgeYears { get; set; }
        [Range(0, 11)]
        public int AgeMonths { get; set; }
        [Range(0, 29)]
        public int AgeDays { get; set; }
    }
}
