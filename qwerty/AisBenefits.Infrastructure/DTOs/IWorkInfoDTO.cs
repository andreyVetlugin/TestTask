using System;

namespace AisBenefits.Infrastructure.DTOs
{
    public interface IWorkInfoDTO
    {
        Guid? RootId { get; set; }

        DateTime StartDate { get; set; }

        DateTime EndDate { get; set; }

        string Organization { get; set; }
        string Function { get; set; }
    }
}
