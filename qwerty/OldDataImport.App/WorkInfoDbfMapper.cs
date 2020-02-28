using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Entities;
using OldDataImport.App.Entities;

namespace OldDataImport.App
{
    public static class WorkInfoDbfMapper
    {
        public static WorkInfo Map(PersonInfo personInfo, Organization organization, Function function, IStag stag)
        {
            var id = Guid.NewGuid();

            return new WorkInfo
            {
                Id = id,
                RootId = id,
                NextId = null,

                CreateTime = DateTime.Now,
                OutdateTime = null,

                PersonInfoRootId = personInfo.RootId,
                FunctionId = function.Id,
                OrganizationId = organization.Id,

                StartDate = stag.DATA_BEGIN,
                EndDate = stag.DATA_END,
            };
        }
    }
}
