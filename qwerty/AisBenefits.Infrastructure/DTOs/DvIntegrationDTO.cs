using System.Collections.Generic;

namespace AisBenefits.Infrastructure.DTOs
{
    public class DvIntegrationDTO
    {
        public string BirthDate { get; set; }
        public string PaymentSum { get; set; }
        public string Udergania { get; set; }
        public IList<DvIntegrationPaymentDTO> Payments { get; set; }
    }

    public class DvIntegrationPaymentDTO
    {
        public string PayDate { get; set; }
        public string PaySum { get; set; }
    }
}