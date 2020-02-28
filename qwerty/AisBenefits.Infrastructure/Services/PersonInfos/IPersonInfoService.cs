using System;
using System.Collections.Generic;
using AisBenefits.Infrastructure.DTOs;
using DataLayer.Entities;

namespace AisBenefits.Infrastructure.Services.PersonInfos
{
    public interface IPersonInfoService
    {
        PersonInfo Create(IPersonInfoDTO personInfoDTO);
        List<PersonInfo> GetAllPersonInfos();
        List<PersonInfo> GetArchive(int pageNumber);
        PersonInfo GetPersonInfo(Guid id);
        void Update(IPersonInfoDTO personInfoDTO);
        void UpdateByWorkInfo(IConfirmExperienceDto confirmExperienceDto);
        string[] GetDocumentTypes();
        int GetNextPersonInfoNumber();
        Guid GetActualVersionId(Guid rootId);
        OperationResult ResumeSolutionForPerson(Guid personInfoRootId);
        void DeactivatePersonInfo(Guid personInfoRootId);

        int GetPagesCount(int itemsCount);
    }
}
