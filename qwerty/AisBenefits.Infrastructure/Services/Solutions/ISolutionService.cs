using AisBenefits.Infrastructure.DTOs;
using DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace AisBenefits.Infrastructure.Services.Solutions
{
   public interface ISolutionService
    {
        void Opredelit(ISolutionForm solutionForm);
        void Count(ISolutionForm solutionForm);
        OperationResult CountFromPensionUpdate(ExtraPay extraPay, DateTime destination, DateTime execution, string comment, bool isElite = false);
        void Pause(ISolutionForm solutionForm);
        void Resume(ISolutionForm solutionForm);
        void Stop(ISolutionForm solutionForm);
        List<Solution> Get(Guid personInfoRootId);
        Solution GetLast(Guid personInfoRootId);
        OperationResult<Guid> DeleteSolution(Guid solutionId, out bool isSolutionActive);
    }
}
