using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.DTOs;

namespace AisBenefits.Models.MinExtraPays
{
    public class MinExtraPayEditForm : IMinExtraPayDto
    {
        public decimal Value { get; set; }
    }
}
