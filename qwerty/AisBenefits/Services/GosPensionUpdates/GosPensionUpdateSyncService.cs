using AisBenefits.Core;
using AisBenefits.Infrastructure.Eip;
using AisBenefits.Infrastructure.Eip.Pfr.PfrBapForPeriodSmev3;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.GosPensions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AisBenefits.Services.GosPensionUpdates
{
    public static class GosPensionUpdateSyncService
    {
        static readonly object SyncLock = new object();

        private static bool Proccessing;
        private static object ProccessingLock = new object();

        public static OperationResult SyncAllGosPensions(this PfrGosPensionContext pfrContext, bool syncNew = true)
        {
            lock (SyncLock)
            {
                bool proccessing;
                lock (ProccessingLock)
                {
                    proccessing = Proccessing;
                }
                if (proccessing)
                    return OperationResult.BuildInnerStateError("Синхронизация госпенсий уже запущена");

                var logger = new FileLogger("c://www/logs/", "sync");

                try
                {
                    Proccessing = true;

                    logger.Log($"Starting the syncing task");
                    Task.Run(() =>
                        {
                            try
                            {
                                logger.Log($"Hey! SyncTask has been started!");
                                using (var readDbContext = DbContextProvider.GetBenefitsReadDbContext(pfrContext.Configuration))
                                using (var writeDbContext = DbContextProvider.GetBenefitsWriteDbContext(pfrContext.Configuration))
                                {
                                    logger.Log($"I'm ready for syncing!");
                                    var modelDataResult = readDbContext.GetGosPensionUpdatesModelData(syncNew);
                                    logger.Log(
                                        $"Wow! I have data!\r\n{JsonConvert.SerializeObject(modelDataResult)}\r\n");
                                    writeDbContext.ProccessGosPensionUpdate(pfrContext.GosPensionPfrClient, modelDataResult);
                                    logger.Log($"Done!");

                                }
                            }
                            catch (Exception e)
                            {
                                logger.Log($"Oh! No! I'm broking!");
                                logger.Log($"I have an exception! Here it is!\r\n{e}\r\n");
                            }
                            finally
                            {
                                logger.Log($"Releasing resources...");
                                lock (ProccessingLock)
                                {
                                    Proccessing = false;
                                }
                                logger.Log($"Exiting...");
                                logger.Dispose();
                            }
                        }
                    );
                }
                catch
                {
                    Proccessing = false;
                }
            }

            return OperationResult.BuildSuccess(UnitOfWork.None());
        }

        public static OperationResult SyncGosPension(this BenefitsAppContext bfsAppContext, PfrGosPensionContext pfrContext, Guid personInfoRootId)
        {
            var gosPensionResult = bfsAppContext.ReadDbContext.GetGosPensionUpdateModelData(personInfoRootId);

            if (!gosPensionResult.Ok)
                OperationResult.BuildErrorFrom(gosPensionResult);

            Enqueue(pfrContext, gosPensionResult.Data);

            return OperationResult.BuildSuccess(UnitOfWork.None());
        }

        public static OperationResult ClearRequests(IPfrBapForPeriodClientConfig config, IEipLogger eipLogger)
        {
            lock (SyncLock)
            {
                bool proccessing;
                lock (ProccessingLock)
                {
                    proccessing = Proccessing;
                }
                if (proccessing)
                    return OperationResult.BuildInnerStateError("Синхронизация госпенсий уже запущена");

                var file = File.OpenWrite("c://www/logs/syncclear.log");
                var fileWriter = new StreamWriter(file, Encoding.UTF8);

                try
                {
                    Proccessing = true;

                    Task.Run(() =>
                    {
                        try
                        {
                            fileWriter.WriteLine($"Clear task has been started!");
                            PfrBapForPeriodClient.Clear(config, eipLogger);
                        }
                        catch (Exception e)
                        {
                            fileWriter.WriteLine($"Oh! No! I'm broking!");
                            fileWriter.WriteLine($"I have an exception! Here it is!\r\n{e}\r\n");
                        }
                        finally
                        {
                            fileWriter.WriteLine($"Releasing resources...");
                            lock (ProccessingLock)
                            {
                                Proccessing = false;
                            }
                            fileWriter.WriteLine($"Exiting...");
                            file.Close();
                        }
                    }
                    );
                }
                catch
                {
                    Proccessing = false;
                }
            }

            return OperationResult.BuildSuccess(UnitOfWork.None());
        }

        static void Enqueue(PfrGosPensionContext pfrContext, GosPensionModelData gosPension)
        {
            if (pfrContext.WorkerState.TryEnqueueGosPension(gosPension) && pfrContext.WorkerState.TryReadyToStart())
                StartWorker(pfrContext);
        }

        static void StartWorker(PfrGosPensionContext pfrContext)
        {
            Task.Run(() =>
            {

            NEXT_ENTRY:
                if (!pfrContext.WorkerState.TryGetNextGosPension(out var gosPensionUpdate))
                    return;

                using (var writeDbContext = DbContextProvider.GetBenefitsWriteDbContext(pfrContext.Configuration))
                {
                    if (gosPensionUpdate.New)
                    {
                        writeDbContext.Add(gosPensionUpdate.Update);
                    }
                    else
                        writeDbContext.Attach(gosPensionUpdate.Update);

                    var startTime = DateTime.Now;
                    while ((DateTime.Now - startTime).Minutes < pfrContext.Config.Timeout - 2)
                    {
                        var result = pfrContext.GosPensionPfrClient.ProccessGosPensionUpdate(
                            gosPensionUpdate,
                            true
                        );

                        if (!result.Ok)
                        {
                            writeDbContext.SaveChanges();
                            goto NEXT_ENTRY;
                        }

                        if (gosPensionUpdate.New)
                        {
                            gosPensionUpdate = new GosPensionModelData(gosPensionUpdate.PersonInfo,
                                gosPensionUpdate.Update,
                                false);
                        }

                        switch (gosPensionUpdate.Update.State)
                        {
                            case DataLayer.Entities.GosPensionUpdateState.Process:
                                break;
                            case DataLayer.Entities.GosPensionUpdateState.Success:
                            case DataLayer.Entities.GosPensionUpdateState.Error:
                                writeDbContext.SaveChanges();
                                pfrContext.GosPensionPfrClient.Complete();
                                goto NEXT_ENTRY;
                        }

                        Thread.Sleep(60 * 1000);
                    }
                }
            });
        }
    }

    public class SyncGosPensionModelData
    {
        public SyncGosPensionState State { get; }
        public string Message { get; }

        public SyncGosPensionModelData(SyncGosPensionState state, string message)
        {
            State = state;
            Message = message;
        }
    }

    public enum SyncGosPensionState
    {
        Ok,
        Timeout,
        Error
    }
}
