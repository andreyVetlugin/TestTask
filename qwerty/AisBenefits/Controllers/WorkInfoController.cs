using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.Logging;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.Organizations;
using AisBenefits.Infrastructure.Services.PersonInfos;
using AisBenefits.Models;
using AisBenefits.Models.Organizations;
using AisBenefits.Models.PersonInfos;
using AisBenefits.Services.Excel;
using AisBenefits.Services.PersonInfos;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/WorkInfo"), ApiController]
    [ApiErrorHandler, ApiModelValidation, PermissionFilter(RolePermissions.AdministratePersonInfos)]
    public class WorkInfoController : ControllerBase
    {
        private readonly IWorkInfoService workInfoService;
        private readonly IPersonInfoService personInfoService;
        private readonly IWorkInfoModelBuilder workInfoModelBuilder;
        private readonly IOrganizationService organizationService;
        private readonly IFunctionService functionService;
        private readonly IWorkExpReferenceModelBuilder workExpReferenceModelBuilder;

        public WorkInfoController(IWorkInfoService workInfoService, IPersonInfoService personInfoService, IWorkInfoModelBuilder workInfoModelBuilder, IOrganizationService organizationService, IFunctionService functionService, IWorkExpReferenceModelBuilder workExpReferenceModelBuilder)
        {
            this.workInfoService = workInfoService;
            this.personInfoService = personInfoService;
            this.workInfoModelBuilder = workInfoModelBuilder;
            this.organizationService = organizationService;
            this.functionService = functionService;
            this.workExpReferenceModelBuilder = workExpReferenceModelBuilder;
        }

        [HttpPost]
        [Route("createOneWorkInfo")]
        public IActionResult CreateOne(WorkInfoFormOne workInfoForm)
        {
            
            var res = workInfoService.CreateOne(workInfoForm.PersonInfoId, workInfoForm.WorkPlace);
            return new ApiOperationResult(res);

        }

        [HttpPost]
        [Route("GetWorkInfo")]
        [GetLogging]
        public IActionResult GetWorkInfo(GuidIdForm guidIdForm)        
        {
            var personInfo = personInfoService.GetPersonInfo(guidIdForm.Id);

            if (personInfo==null)
            {
                return new ApiResult(new { Message = "Такая карточка не найдена увы" }, 404);
            }

            var workInfoModel = workInfoModelBuilder.Build(personInfo);

            return ApiModelResult.Create(workInfoModel);
        }

        [HttpPost]
        [Route("GetExcel")]
        [GetLogging]
        public IActionResult GetExcel(GuidIdForm guidIdForm)
        {
            var personInfo = personInfoService.GetPersonInfo(guidIdForm.Id);

            if (personInfo == null)
            {
                return new ApiResult(new { Message = "Такая карточка не найдена увы" }, 404);
            }

            var workInfoModelResult = workInfoModelBuilder.Build(personInfo);
            if(!workInfoModelResult.Ok)
                return ApiModelResult.Create(workInfoModelResult);

            var res = workExpReferenceModelBuilder.BuildTotal(personInfo, workInfoModelResult.Data);
            return res;
        }




        [HttpGet]
        [Route("GetAllWorkInfos")]
        [GetLogging]
        public IActionResult GetAllWorkInfos()        
        {
            return new ApiResult(workInfoService.GetAll());
        }              

        [HttpPost]
        [Route("UpdateOneWorkInfo")]
        public IActionResult UpdateOneWorkInfo(WorkInfoFormOne workInfoForm)
        {
            var res = workInfoService.UpdateOne(workInfoForm.WorkPlace);
            return new ApiOperationResult(res);
        }

        [HttpPost]
        [Route("DeleteOneWorkInfo")]
        public IActionResult DeleteOneWorkInfo(GuidIdForm data)
        {
            var res = workInfoService.DeleteOne(data.Id);
            return new ApiOperationResult(res);
        }


        [HttpPost]
        [Route("ConfirmExperience")]
        public void ConfirmExperience(ConfirmExperienceForm form)
        {
            personInfoService.UpdateByWorkInfo(form);
        }


        [HttpGet]
        [Route("GetAllOrganizations")]
        [GetLogging]
        public IActionResult GetAllOrganizations()
        {
            var orgs = organizationService.GetAll();
            var funcs = functionService.GetAll();
            var model = new OrganizationsAndFunctionsModel
            {
                Functions = funcs.ToArray(),
                Organizations = orgs.ToArray()
            };
            return new ApiResult(model);
        }

    }
}