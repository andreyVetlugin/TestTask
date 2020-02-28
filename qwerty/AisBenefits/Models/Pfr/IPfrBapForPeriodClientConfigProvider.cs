using AisBenefits.Infrastructure.Eip.Pfr.PfrBapForPeriodSmev3;

namespace AisBenefits.Models.Pfr
{
    public interface IPfrBapForPeriodClientConfigProvider
    {
        PfrBapForPeriodClientConfig Get();
    }

    public class PfrBapForPeriodClientConfig : IPfrBapForPeriodClientConfig
    {
        public string Uri { get; set; }
        public string FrguCode { get; set; }
        public bool SignMessage { get; set; }
        public string CertificateThumbprint { get; set; }

        public int LogLevel { get; set; }
        public string LogDirectory { get; set; }

        public int Timeout { get; set; }
    }
}