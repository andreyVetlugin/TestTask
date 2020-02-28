using AisBenefits.Infrastructure.DTOs;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.Services.GosPensions
{
    public interface IGosPensionUpdateService
    {
        IOperationResult ApproveOne(Guid updateId, DateTime destination, DateTime execution, string comment);
        IOperationResult ApproveMany(IAcceptedPensionUpdateDto incomePensionForms);
        void Decline(IDeclinedPensionsDTO[] declinedPensionsDTOs);
        void DeclineOne(IDeclinedPensionsDTO declinedPensionsDTO, bool saveDb = true);
        List<GosPensionUpdate> GetIncomePensions();

    }
}
