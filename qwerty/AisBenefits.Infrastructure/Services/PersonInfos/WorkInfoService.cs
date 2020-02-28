using AisBenefits.Infrastructure.DTOs;
using AisBenefits.Infrastructure.Services.ExtraPays;
using AisBenefits.Infrastructure.Services.Solutions;
using AisBenefits.Infrastructure.Services.WorkInfos;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using AisBenefits.Infrastructure.Services.Functions;
using AisBenefits.Infrastructure.Services.Organizations;

namespace AisBenefits.Infrastructure.Services.PersonInfos
{
    public class WorkInfoService : IWorkInfoService
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly ICurrentUserProvider currentUserProvider;
        private readonly ISolutionService solutionService;
        private readonly ILogBuilder logBuilder;

        public WorkInfoService(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext, ICurrentUserProvider currentUserProvider, ISolutionService solutionService, ILogBuilder logBuilder)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
            this.currentUserProvider = currentUserProvider;
            this.solutionService = solutionService;
            this.logBuilder = logBuilder;
        }

        public OperationResult CreateOne(Guid personInfoId, IWorkInfoDTO workPlaceDto)
        {
            readDbContext.EnsureOrganizationExist(writeDbContext, workPlaceDto.Organization,
                out var organization);
            readDbContext.EnsureFunctionExist(writeDbContext, workPlaceDto.Function,
                out var function);

            var workPlaceId = Guid.NewGuid();
            var workPlace = new WorkInfo
            {
                Id = workPlaceId,
                RootId = workPlaceId,
                PersonInfoRootId = personInfoId,
                CreateTime = DateTime.Now,

                StartDate = workPlaceDto.StartDate,
                EndDate = workPlaceDto.EndDate,

                OrganizationId = organization.Id,
                FunctionId = function.Id,
            };
            workPlace.PersonInfoRootId = personInfoId;
            workPlace.RootId = Guid.NewGuid();
            workPlace.Id = workPlace.RootId;
            workPlace.CreateTime = DateTime.Now;

            writeDbContext.Add(workPlace);


            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.WorkInfoCREATE, workPlace.RootId); //Логгирование в табличку
            writeDbContext.Add(infoLog);

            var res = UpdateExtraPay(readDbContext.GetWorkInfosModelData(workPlace, WorkInfosModelDataType.Add, organization));

            if (!res.Ok)
                return res;

            return OperationResult.BuildSuccess(
                UnitOfWork.Complex(
                    UnitOfWork.WriteDbContext(writeDbContext),
                    res)
                );
        }



        public OperationResult DeleteOne(Guid workInfoRootId)
        {
            var modelDataResult = readDbContext.GetWorkInfoModelData(workInfoRootId);

            if (!modelDataResult.Ok)
                return OperationResult.BuildErrorFrom(modelDataResult);

            var workInfo = modelDataResult.Data;

            writeDbContext.Attach(workInfo.Instance);
            workInfo.Instance.NextId = default(Guid);
            workInfo.Instance.OutdateTime = DateTime.Now;

            var res = UpdateExtraPay(readDbContext.GetWorkInfosModelData(workInfo.Instance, WorkInfosModelDataType.Remove));

            if (!res.Ok)
                return res;

            return OperationResult.BuildSuccess(
                UnitOfWork.Complex(
                    UnitOfWork.WriteDbContext(writeDbContext),
                    res)
            );
        }



        public WorkInfo[] Get(Guid personInfoId)
        {
            var c = readDbContext.Get<WorkInfo>().ByPersonInfoId(personInfoId);
            return c.ToArray();
        }

        public List<WorkInfo> GetAll()
        {
            return readDbContext.Get<WorkInfo>().ToList();
        }

        public OperationResult UpdateOne(IWorkInfoDTO place)
        {
            if (place.RootId == null)
            {
                throw new Exception("вам не сюда");
            }

            var modelDataResult = readDbContext.GetWorkInfoModelData(place.RootId.Value);

            if (!modelDataResult.Ok)
                return OperationResult.BuildErrorFrom(modelDataResult);

            var workInfo = modelDataResult.Data;

            readDbContext.EnsureOrganizationExist(writeDbContext, place.Organization,
                out var organization);
            readDbContext.EnsureFunctionExist(writeDbContext, place.Function,
                out var function);

            var workPlace = new WorkInfo
            {
                Id = Guid.NewGuid(),
                RootId = workInfo.Instance.RootId,
                PersonInfoRootId = workInfo.Instance.PersonInfoRootId,
                CreateTime = DateTime.Now,

                StartDate = place.StartDate,
                EndDate = place.EndDate,

                OrganizationId = organization.Id,
                FunctionId = function.Id,
            };

            writeDbContext.Add(workPlace);

            writeDbContext.Attach(workInfo.Instance);
            workInfo.Instance.NextId = workPlace.Id;
            workInfo.Instance.OutdateTime = DateTime.Now;

            writeDbContext.Add(workPlace);

            var infoLog =
                logBuilder.BuildPostInfoLog(PostOperationType.WorkInfoUPDATE,
                    workPlace.RootId); //Логгирование в табличку
            writeDbContext.Add(infoLog);

            var res = UpdateExtraPay(readDbContext.GetWorkInfosModelData(workPlace, WorkInfosModelDataType.Edit, organization));

            if (!res.Ok)
                return res;

            return OperationResult.BuildSuccess(
                UnitOfWork.Complex(
                    UnitOfWork.WriteDbContext(writeDbContext),
                    res)
                );
        }

        private OperationResult UpdateExtraPay(ModelDataResult<WorkInfosModelData> workInfosResult)
        {
            return OperationResult.BuildSuccess(UnitOfWork.None()); //TODO - удалить когда поправят баг с пересчётом
            var modelDataResult = readDbContext.GetExtraPayModelData(workInfosResult);
            if (!modelDataResult.Ok)
                return OperationResult.BuildErrorFrom(modelDataResult);

            if (modelDataResult.Data.Initial) return OperationResult.BuildSuccess(UnitOfWork.None());

            var extraPay = modelDataResult.Data;

            var currentUser = currentUserProvider.GetCurrentUser();
            var form = ExtraPayRecalculateForm.CreateDsPerc(extraPay.Instance.PersonRootId);

            var editModelDataResult = readDbContext.GetEditExtraPayData(form, ExtraPayRecalculateType.WorkAge, currentUser, modelDataResult);

            var operationResult = writeDbContext.RecalculateExtraPay(editModelDataResult);

            var solutionResult = solutionService.CountFromPensionUpdate(editModelDataResult.Data.NewExtraPay, DateTime.Now,
                DateTime.Now, "Добавлено в связи с обновлением опыта работы");

            return OperationResult.BuildFromResults(operationResult, solutionResult);
        }

    }
}
