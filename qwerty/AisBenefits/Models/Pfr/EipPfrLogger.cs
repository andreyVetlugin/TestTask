using AisBenefits.Core;
using AisBenefits.Infrastructure.Eip;
using System;

namespace AisBenefits.Models.Pfr
{
    public class EipPfrLogger : IEipLogger
    {
        private readonly PfrBapForPeriodClientConfig config;

        public EipPfrLogger(IPfrBapForPeriodClientConfigProvider configProvider)
        {
            this.config = configProvider.Get();
        }

        public void Error(string context, string error)
        {
            Log(context, error, 0);
        }

        public void Info(string context, string info)
        {
            Log(context, info, 2);
        }

        private void Log(string context, string content, int logLevel)
        {
            if (config.LogLevel >= logLevel)
                using (var logger = new FileLogger(config.LogDirectory, "pfr", DateTime.Now))
                {
                    logger.Log($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {context} LogLevel={logLevel}\r\n{content}");
                }
        }
    }

    public class EipLogger : IEipLogger
    {
        private readonly string directory;
        private readonly int logLevel;

        public EipLogger(string directory, int logLevel)
        {
            this.directory = directory;
            this.logLevel = logLevel;
        }

        public void Error(string context, string error)
        {
            Log(context, error, 0);
        }

        public void Info(string context, string info)
        {
            Log(context, info, 2);
        }

        private void Log(string context, string content, int logLevel)
        {
            if (this.logLevel >= logLevel)
                using (var logger = new FileLogger(directory, context, DateTime.Now))
                {
                    logger.Log($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {context} LogLevel={logLevel}\r\n{content}");
                }
        }
    }
}
