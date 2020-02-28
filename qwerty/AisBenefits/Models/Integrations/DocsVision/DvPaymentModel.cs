using System.Collections.Generic;

namespace AisBenefits.Models.Integrations.DocsVision
{
    public class DvPaymentModel
    {
        public DvResponseStatus Status { get; set; }
        public string BirthDate { get; set; }
        public string PaymentSum { get; set; }
        public string Udergania { get; set; }
        public IList<DvPayment> Payments { get; set; }
        public string ErrMess { get; set; }
    }
}