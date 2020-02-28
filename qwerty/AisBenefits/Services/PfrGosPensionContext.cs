using System.Collections.Generic;
using System.Linq;
using AisBenefits.Infrastructure.Eip;
using AisBenefits.Infrastructure.Eip.Pfr.PfrBapForPeriodSmev3;
using AisBenefits.Infrastructure.Services.GosPensions;
using Microsoft.Extensions.Configuration;

namespace AisBenefits.Services
{
    public class PfrGosPensionContext
    {
        public PfrGosPensionWorkerState WorkerState { get; }

        public IConfiguration Configuration { get; }
        public IPfrBapForPeriodClientConfig Config { get; }
        public IEipLogger EipLogger { get; }

        public IGosPensionPfrClient GosPensionPfrClient { get; }

        public PfrGosPensionContext(PfrGosPensionWorkerState workerState, IConfiguration configuration, IPfrBapForPeriodClientConfig config, IEipLogger eipLogger, IGosPensionPfrClient gosPensionPfrClient)
        {
            WorkerState = workerState;
            Configuration = configuration;
            Config = config;
            EipLogger = eipLogger;
            GosPensionPfrClient = gosPensionPfrClient;
        }
    }

    public class PfrGosPensionWorkerState
    {
        public bool TryEnqueueGosPension(GosPensionModelData gosPension)
        {
            lock (workerLock)
            {
                if ((currentGosPension != null && currentGosPension.PersonInfo.RootId == gosPension.PersonInfo.RootId) ||
                    gosPensionQueue.Any(g => g.PersonInfo.RootId == gosPension.PersonInfo.RootId))
                    return false;

                gosPensionQueue.Enqueue(gosPension);
                return true;
            }
        }

        public bool TryGetNextGosPension(out GosPensionModelData gosPension)
        {
            lock (workerLock)
            {
                var r = gosPensionQueue.TryDequeue(out gosPension);
                currentGosPension = gosPension;

                if (!r)
                    workerIsRunning = false;

                return r;
            }
        }

        public bool Running
        {
            get
            {
                lock (workerLock)
                {
                    return workerIsRunning;
                }
            }
        }

        public bool TryReadyToStart()
        {
            lock (workerLock)
            {
                if (!workerIsRunning && gosPensionQueue.Count > 0)
                {
                    workerIsRunning = true;
                    return true;
                }

                return false;
            }
        }

        object workerLock = new object();

        Queue<GosPensionModelData> gosPensionQueue = new Queue<GosPensionModelData>();
        private GosPensionModelData currentGosPension;
        bool workerIsRunning;
    }
}
