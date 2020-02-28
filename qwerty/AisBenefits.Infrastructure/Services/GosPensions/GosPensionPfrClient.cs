using System.Collections.Generic;
using AisBenefits.Infrastructure.Eip;
using AisBenefits.Infrastructure.Eip.Pfr.PfrBapForPeriodSmev3;

namespace AisBenefits.Infrastructure.Services.GosPensions
{
    public class GosPensionPfrClient : IGosPensionPfrClient
    {
        private readonly IPfrBapForPeriodClientConfig config;
        private readonly IEipLogger logger;

        public GosPensionPfrClient(IPfrBapForPeriodClientConfig config, IEipLogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        List<IPfrBapForPeriodClientRequestTarget> deleteRequestQueue = new List<IPfrBapForPeriodClientRequestTarget>();
        public void DeleteRequest(IPfrBapForPeriodClientRequestTarget target)
        {
            deleteRequestQueue.Add(target);
        }

        public PfrBapForPeriodClientRequestInitial RequestInitial(IPfrBapForPeriodClientRequestTarget target)
        {
            return PfrBapForPeriodClient.RequestInitial(target, config, logger);
        }

        public PfrBapForPeriodClientRequestResult RequestResult(IPfrBapForPeriodClientRequestTarget target)
        {
            return PfrBapForPeriodClient.RequestResult(target, config, logger);
        }

        public void Complete()
        {
            foreach (var target in deleteRequestQueue)
                PfrBapForPeriodClient.DeleteRequest(target, config, logger);

            deleteRequestQueue.Clear();
        }
    }
}
