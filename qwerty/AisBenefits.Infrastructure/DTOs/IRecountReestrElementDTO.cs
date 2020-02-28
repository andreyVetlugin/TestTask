using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.DTOs
{
    public interface IRecountReestrElementDTO
    {
        Guid ReestrElementId { get; set; }
        DateTime? From { get; set; }
        DateTime? To { get; set; }
        
        decimal NewSumm { get; set; }
    }
}
