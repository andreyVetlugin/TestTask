using System;

namespace AisBenefits.Services
{
    public class BackgroundContext
    {
        public IServiceProvider ServiceProvider { get; }
        public ProcessState State { get; }

        public BackgroundContext(IServiceProvider serviceProvider, ProcessState state)
        {
            ServiceProvider = serviceProvider;
            State = state;
        }

        public class ProcessState
        {
            public bool LifecycleProcessStarted { get; set; }
        }

    }
}
