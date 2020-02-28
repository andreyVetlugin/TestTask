using System;
using System.Collections.Generic;
using AisBenefits.Infrastructure.Eip.Pfr.PfrBapForPeriodSmev3;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.GosPensions
{
    using GosPensionModelDataResult = ModelDataResult<GosPensionModelData>;
    using GosPensionsModelDataResult = ModelDataResult<List<GosPensionModelData>>;

    public static class GosPensionOperationHelper
    {
        public static OperationResult ProccessGosPensionUpdate(
            this IWriteDbContext<IBenefitsEntity> writeDbContext, IGosPensionPfrClient client, GosPensionsModelDataResult modelDataResult)
        {
            if (!modelDataResult.Ok)
                return OperationResult.BuildErrorFrom(modelDataResult);

            foreach (var data in modelDataResult.Data)
            {
                writeDbContext.ProccessGosPensionUpdate(client, data);
            }

            return OperationResult.BuildSuccess(UnitOfWork.None());
        }
        public static OperationResult ProccessGosPensionUpdate(
    this IWriteDbContext<IBenefitsEntity> writeDbContext, IGosPensionPfrClient client, GosPensionModelData data)
        {
            if (data.New)
                writeDbContext.Add(data.Update);
            else
                writeDbContext.Attach(data.Update);

            client.ProccessGosPensionUpdate(data, false).Complete();
            writeDbContext.SaveChanges();

            return OperationResult.BuildSuccess(UnitOfWork.None());
        }

        public static OperationResult ProccessGosPensionUpdate(this IGosPensionPfrClient client, GosPensionModelData gosPension, bool force)
        {
            var personInfo = gosPension.PersonInfo;
            var update = gosPension.Update;

            var target = new Target(personInfo, update);

            if (update.State != GosPensionUpdateState.Process && update.State != GosPensionUpdateState.Error && !force)
                return OperationResult.BuildSuccess(client);

            if (gosPension.New || update.State == GosPensionUpdateState.Error || update.State == GosPensionUpdateState.Success && force)
            {
                var result = client.RequestInitial(target);

                if (result.Ok)
                {
                    update.State = GosPensionUpdateState.Process;
                    update.Approved = false;
                    update.Declined = false;

                    return OperationResult.BuildSuccess(client);
                }
            }
            else
            {
                var result = client.RequestResult(target);
                if (result.Done)
                {
                    update.Amount = result.GosPensionAmount;
                    update.State = GosPensionUpdateState.Success;

                    client.DeleteRequest(target);
                    return OperationResult.BuildSuccess(client);
                }
                else if (!result.Ok)
                {
                    update.State = GosPensionUpdateState.Error;
                }
            }

            return OperationResult.BuildSuccess(client);
        }

        public class Target : IPfrBapForPeriodClientRequestTarget
        {
            private readonly PersonInfo personInfo;
            private readonly GosPensionUpdate update;

            public Target(PersonInfo personInfo, GosPensionUpdate update)
            {
                this.personInfo = personInfo;
                this.update = update;
            }

            public string MiddleName => personInfo.MiddleName;
            public string FirstName => personInfo.Name;
            public string SurName => personInfo.SurName;
            public string Snils => personInfo.SNILS;
            public DateTime BirthDate => personInfo.BirthDate;
            public string Gender => personInfo.Sex.ToString();
            public DateTime Date => update.Date;
            public Guid RequestId => update.Id;
        }
    }

    public class GosPensionUpdateProccessData
    {
        public GosPensionModelData GosPensionUpdate { get; }

        public bool Done { get; }
        public decimal GosPensionAmout { get; }

        public GosPensionUpdateProccessData(GosPensionModelData gosPensionUpdate, bool done, decimal gosPensionAmout)
        {
            GosPensionUpdate = gosPensionUpdate;
            Done = done;
            GosPensionAmout = gosPensionAmout;
        }
    }
}
