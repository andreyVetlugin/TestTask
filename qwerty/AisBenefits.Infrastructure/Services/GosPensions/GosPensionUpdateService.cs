using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AisBenefits.Infrastructure.DTOs;
using AisBenefits.Infrastructure.Services.ExtraPays;
using AisBenefits.Infrastructure.Services.Solutions;
using AisBenefits.Infrastructure.Services.WorkInfos;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.GosPensions
{
    public class GosPensionUpdateService : IGosPensionUpdateService
    {

        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly ICurrentUserProvider currentUserProvider;
        private readonly ISolutionService solutionService;
        private readonly ILogBuilder logBuilder;

        public GosPensionUpdateService(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext, ISolutionService solutionService, ILogBuilder logBuilder, ICurrentUserProvider currentUserProvider)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
            this.solutionService = solutionService;
            this.logBuilder = logBuilder;
            this.currentUserProvider = currentUserProvider;
        }

        public IOperationResult ApproveOne(Guid updateId, DateTime destination, DateTime execution, string comment)
        {
                var incomePension = readDbContext.Get<GosPensionUpdate>().ById(updateId).FirstOrDefault();
                if(incomePension == null)
                    return OperationResult.BuildFormError("нет такого в базе");

                var personInfo = readDbContext.Get<PersonInfo>()
                    .ByRootId(incomePension.PersonInfoRootId)
                    .FirstOrDefault();
                var workInfosResult = readDbContext.GetWorkInfosModelData(personInfo);
                var extraPayModelDataResult = readDbContext.GetExtraPayModelData(workInfosResult);

                var extraPayEditModelDataResult = readDbContext.GetEditExtraPayData(
                    ExtraPayRecalculateForm.CreateGosPension(incomePension.PersonInfoRootId, incomePension.Amount),
                    ExtraPayRecalculateType.GosPension,
                    currentUserProvider.GetCurrentUser(),
                    extraPayModelDataResult);

                var operationResult = writeDbContext.RecalculateExtraPay(extraPayEditModelDataResult);

                if (!operationResult.Ok)
                    return operationResult;

                writeDbContext.Attach(incomePension);
                incomePension.Approved = true;

                solutionService.CountFromPensionUpdate(extraPayEditModelDataResult.Data.NewExtraPay, destination,
                    execution, comment);

            var infoLog1 = logBuilder.BuildPostInfoLog(PostOperationType.IncomePensionApproved, incomePension.Id);
            writeDbContext.Add(infoLog1);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        public IOperationResult ApproveMany(IAcceptedPensionUpdateDto incomePensionForms)
        {
            foreach (var incomeUpdate in incomePensionForms.GosPensionUpdateIds)
            {
                var dest = incomePensionForms.Destination;
                var execution = incomePensionForms.Execution;
                var comment = incomePensionForms.Comment;

                var result = ApproveOne(incomeUpdate, dest, execution, comment);
                if (!result.Ok)
                    return result;
            }

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        public void Decline(IDeclinedPensionsDTO[] declinedPensionsDTOs)
        {
            foreach (var declinedPension in declinedPensionsDTOs)
            {
                DeclineOne(declinedPension, false);
            }

            writeDbContext.SaveChanges();
        }

        public void DeclineOne(IDeclinedPensionsDTO declinedPensionDTO, bool saveDb = true) // Флаг - сохранять ли базу в конце
        {
            var declinedPensionId = declinedPensionDTO.IncomePensionId;

            var incomePension = readDbContext.Get<GosPensionUpdate>().ById(declinedPensionId).FirstOrDefault();

            if (incomePension == null)
            {
                throw new Exception(message: "нет такого в базе");
            }

            writeDbContext.Attach(incomePension);
            incomePension.Declined = true;

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.IncomePensionDeclined, incomePension.Id); //Логгирование
            writeDbContext.Add(infoLog);

            if (saveDb)
            {
                writeDbContext.SaveChanges();
            }

        }

        public List<GosPensionUpdate> GetIncomePensions()
        {
            var date = DateTime.Now;
            var pensions = readDbContext.Get<GosPensionUpdate>().SuccessActualAt(date.Year, date.Month).ToList();
            return pensions;
        }
    }
}
