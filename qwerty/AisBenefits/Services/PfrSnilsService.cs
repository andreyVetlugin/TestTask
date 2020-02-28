using AisBenefits.Core;
using AisBenefits.Infrastructure.Eip.Pfr.Snils;
using AisBenefits.Infrastructure.Services.PostInfoLogs;
using DataLayer.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AisBenefits.Services
{
    public class PfrSnilsService: IDisposable
    {
        private readonly PfrSnilsClient client;
        private readonly IConfiguration configuration;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();

        private readonly ConcurrentDictionary<Guid, PfrSnilsRequestTarget> inProcess = new ConcurrentDictionary<Guid, PfrSnilsRequestTarget>();

        private readonly object runLock = new object();

        public PfrSnilsService(PfrSnilsClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;

            CheckBackupRequests();
        }

        public PfrSnilsServiceEnqueueResult Enqueue(PersonInfo person)
        {
            if (inProcess.ContainsKey(person.RootId))
                return PfrSnilsServiceEnqueueResult.InProcess;

            if(client.RequestInitial(new PfrSnilsRequestTarget(person,
                new PfrSnilsRequest
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    PersonInfoRootId = person.RootId
                },
                this)
                    ))
            {
                RunRequestResults();
                return PfrSnilsServiceEnqueueResult.Enqueued;
            }

            return PfrSnilsServiceEnqueueResult.Error;
        }

        private void CheckBackupRequests()
        {
            using(var read = configuration.GetBenefitsReadDbContext())
            {
                foreach (var target in read.Get<PfrSnilsRequest>().Sended()
                    .AsEnumerable()
                    .Select(r => new PfrSnilsRequestTarget(null, r, this)))
                    inProcess[target.request.PersonInfoRootId] = target;

                if (inProcess.Count > 0)
                    RunRequestResults();
            }
        }

        private void RunRequestResults()
        {
            if (Monitor.TryEnter(runLock))
            {
                Task.Run(() =>
                {
                    try
                    {
                        while (inProcess.Count > 0)
                        {
                            foreach (var target in inProcess.Values.ToList())
                                client.RequestResult(target);

                            if (!cancellation.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(3)))
                                break;
                        }
                    }
                    finally
                    {
                        Monitor.Exit(runLock);
                    }
                });
            }
        }

        private void OnSended(PfrSnilsRequestTarget target)
        {
            using (var write = configuration.GetBenefitsWriteDbContext())
            {
                var request = target.request;
                write.Add(request);

                write.SaveChanges();

                inProcess[request.PersonInfoRootId] = target;
            }
        }
        private void OnSnils(PfrSnilsRequestTarget target, string snils)
        {
            using (var read = configuration.GetBenefitsReadDbContext())
            using (var write = configuration.GetBenefitsWriteDbContext())
            {
                var request = target.request;
                write.Attach(request);

                request.State = PfrSnilsRequestState.Success;
                request.Snils = snils;

                var person = read.Get<PersonInfo>().ByRootId(request.PersonInfoRootId).FirstOrDefault();
                write.Attach(person);

                var newPerson = person.CreateNext(Guid.NewGuid(), DateTime.Now);
                newPerson.SNILS = snils;

                write.Add(newPerson);
                write.CreatePostInfoLog(PostOperationType.PfrSnilsUpdate, newPerson.RootId, Guid.Empty);

                write.SaveChanges();

                inProcess.TryRemove(request.PersonInfoRootId, out var removed);
            }
        }
        private void OnError(PfrSnilsRequestTarget target)
        {
            using (var write = configuration.GetBenefitsWriteDbContext())
            {
                var request = target.request;
                write.Attach(request);

                request.State = PfrSnilsRequestState.Error;

                write.SaveChanges();

                inProcess.TryRemove(request.PersonInfoRootId, out var removed);
            }
        }

        public void Dispose()
        {
            cancellation.Cancel();
        }

        class PfrSnilsRequestTarget : IPfrSnilsRequestTarget
        {
            private readonly PersonInfo personInfo;
            public readonly PfrSnilsRequest request;
            private readonly PfrSnilsService service;

            public PfrSnilsRequestTarget(PersonInfo personInfo, PfrSnilsRequest request, PfrSnilsService service)
            {
                this.personInfo = personInfo;
                this.request = request;
                this.service = service;
            }

            Guid IPfrSnilsRequestTarget.Id => request.Id;

            string IPfrSnilsRequestTarget.FirstName => personInfo.Name;

            string IPfrSnilsRequestTarget.SurName => personInfo.SurName;

            string IPfrSnilsRequestTarget.MiddleName => personInfo.MiddleName;

            DateTime IPfrSnilsRequestTarget.BirthDate => personInfo.BirthDate;

            string IPfrSnilsRequestTarget.Gender => personInfo.Sex.ToString();

            string IPfrSnilsRequestTarget.DocType => personInfo.DocTypeId;

            string IPfrSnilsRequestTarget.DocSeria => personInfo.DocSeria;

            string IPfrSnilsRequestTarget.DocNumber => personInfo.DocNumber;

            DateTime IPfrSnilsRequestTarget.DocIssueDate => personInfo.IssueDate ?? DateTime.Now;

            string IPfrSnilsRequestTarget.DocIssuePlace => personInfo.Issuer;

            void IPfrSnilsRequestTarget.SetError()
            {
                service.OnError(this);
            }

            void IPfrSnilsRequestTarget.SetSended()
            {
                service.OnSended(this);
            }

            void IPfrSnilsRequestTarget.SetSnils(string snils)
            {
                service.OnSnils(this, snils);
            }
        }
    }

    public enum PfrSnilsServiceEnqueueResult
    {
        Enqueued,
        InProcess,
        Error
    }

    public class PfrSnilsClientConfig
    {
        public string Uri { get; set; }
        public string FrguCode { get; set; }
        public string LogDirectory { get; set; }
        public int LogLevel { get; set; }
    }
}
