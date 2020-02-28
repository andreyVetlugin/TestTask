using AisBenefits.Infrastructure.DTOs;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.Services.PersonInfos
{
    public interface IWorkInfoService
    {
        OperationResult CreateOne(Guid personInfoId, IWorkInfoDTO workPlaceDTO);
        OperationResult UpdateOne(IWorkInfoDTO workPlaceDTO);
        OperationResult DeleteOne(Guid workInfoRootId);
        WorkInfo[] Get(Guid personInfoId);
        List<WorkInfo> GetAll();
    }
}
