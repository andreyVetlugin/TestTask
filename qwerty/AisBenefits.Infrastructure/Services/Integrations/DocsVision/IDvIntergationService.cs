using System;
using AisBenefits.Infrastructure.DTOs;

namespace AisBenefits.Infrastructure.Services.Integrations.DocsVision
{
    public interface IDvIntergationService
    {
        DvIntegrationDTO GetPaymentsByPersonData(string surname, string name, string patronymic, DateTime? birthdate, DateTime start, DateTime end);
    }
}