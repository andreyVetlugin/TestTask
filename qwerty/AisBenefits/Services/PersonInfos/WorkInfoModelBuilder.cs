using AisBenefits.Infrastructure.DTOs;
using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Models.PersonInfos;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.WorkInfos;

namespace AisBenefits.Services.PersonInfos
{
    public interface IWorkInfoModelBuilder
    {
        ModelDataResult<WorkInfoModel> Build(PersonInfo personInfo);
    }

    public class WorkInfoModelBuilder: IWorkInfoModelBuilder
    {

        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public WorkInfoModelBuilder(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public ModelDataResult<WorkInfoModel> Build(PersonInfo personInfo)
        {
            var workInfosResult = readDbContext.GetWorkInfosModelData(personInfo);

            if(!workInfosResult.Ok)
                return ModelDataResult<WorkInfoModel>.BuildErrorFrom(workInfosResult);

            var workInfos = workInfosResult.Data;

            var functions = readDbContext.Get<Function>()
                .ByIds(workInfos.WorkInfos.Select(w => w.Instance.FunctionId))
                .ToDictionary(f => f.Id);

            var data = new WorkInfoModel
            {
                PersonInfoRootId = workInfos.PersonInfoRootId,
                AgeDays = workInfos.WorkAge.Days,
                AgeMonths = workInfos.WorkAge.Months,
                AgeYears = workInfos.WorkAge.Years,
                Approved = personInfo.Approved,
                DocsSubmitDate = personInfo.DocsSubmitDate,
                DocsDestinationDate = personInfo.DocsDestinationDate,
                WorkPlaces = workInfos.WorkInfos.Select(w => new WorkPlaceModel
                {
                    RootId = w.Instance.RootId,
                    AgeDays = w.WorkAge.Days,
                    AgeMonths = w.WorkAge.Months,
                    AgeYears = w.WorkAge.Years,
                    OrganizationId = w.Organization.Id,
                    OrganizationName = w.Organization.OrganizationName,
                    Function = functions[w.Instance.FunctionId].Name,
                    FunctionId = w.Instance.FunctionId,
                    StartDate = w.Instance.StartDate,
                    EndDate = w.Instance.EndDate
                })
                .OrderByDescending(w => w.EndDate)
                .ToArray()
            };

            return ModelDataResult<WorkInfoModel>.BuildSucces(data);
        }
    }
}
