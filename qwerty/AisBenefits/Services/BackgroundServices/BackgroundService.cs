using System;
using System.Threading;
using System.Threading.Tasks;
using AisBenefits.Services.GosPensionUpdates;
using Microsoft.Extensions.DependencyInjection;

namespace AisBenefits.Services.BackgroundServices
{
    public static class BackgroundService
    {
        public static void StartLifecycleProcess(this BackgroundContext context)
        {
            Task.Run(() =>
            {
                lock (context)
                {
                    if (context.State.LifecycleProcessStarted)
                        return;
                    context.State.LifecycleProcessStarted = true;
                }
                var serviceProvider = context.ServiceProvider;
                
                for (;;)
                    try
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            var pfrGosPensionContext = scope.ServiceProvider.GetService<PfrGosPensionContext>();
                            pfrGosPensionContext.SyncAllGosPensions(false);
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(30));
                    }
            });
        }
    }
}
