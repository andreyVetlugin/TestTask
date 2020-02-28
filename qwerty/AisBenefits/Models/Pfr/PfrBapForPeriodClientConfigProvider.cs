using Microsoft.Extensions.Configuration;

namespace AisBenefits.Models.Pfr
{
    public class PfrBapForPeriodClientConfigProvider : IPfrBapForPeriodClientConfigProvider
    {
        private readonly IConfiguration configuration;

        public PfrBapForPeriodClientConfigProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public PfrBapForPeriodClientConfig Get() =>
            configuration.GetSection("EipPfr:BapForPeriod")
                .Get<PfrBapForPeriodClientConfig>();
    }
}
