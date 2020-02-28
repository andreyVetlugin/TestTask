using System.Linq;
using AisBenefits.ActionResults;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Attributes.Logging;
using AisBenefits.Infrastructure.Services.DropDowns;
using AisBenefits.Infrastructure.Services.PersonInfos;
using AisBenefits.Models;
using AisBenefits.Models.PersonInfos;
using AisBenefits.Services.PersonInfos;
using Microsoft.AspNetCore.Mvc;
using AisBenefits.Attributes;
using AisBenefits.Services.Word;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using AisBenefits.Services;
using System;

namespace AisBenefits.Controllers
{
    [Route("/api/personinfo")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePersonInfos)]
    public class PersonInfoController : Controller
    {
        private readonly IPersonInfoService personInfoService;
        private readonly IPersonInfoModelBuilder personInfoModelBuilder;
        private readonly IPersonInfoPreviewModelBuilder personInfoPreviewModelBuilder;
        private readonly IPersonInfoWordModelBuilder personInfoWordModelBuilder;
        private readonly IWorkInfoService workInfoService;
        private readonly IWorkInfoModelBuilder workInfoModelBuilder;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly PfrSnilsService pfrSnilsService;

        public PersonInfoController(IPersonInfoService personInfoService, IPersonInfoModelBuilder personInfoModelBuilder, IPersonInfoPreviewModelBuilder personInfoPreviewModelBuilder, IPersonInfoWordModelBuilder personInfoWordModelBuilder, IWorkInfoService workInfoService, IWorkInfoModelBuilder workInfoModelBuilder, IReadDbContext<IBenefitsEntity> readDbContext, PfrSnilsService pfrSnilsService)
        {
            this.personInfoService = personInfoService;
            this.personInfoModelBuilder = personInfoModelBuilder;
            this.personInfoPreviewModelBuilder = personInfoPreviewModelBuilder;
            this.personInfoWordModelBuilder = personInfoWordModelBuilder;
            this.workInfoService = workInfoService;
            this.workInfoModelBuilder = workInfoModelBuilder;
            this.readDbContext = readDbContext;
            this.pfrSnilsService = pfrSnilsService;
        }

        [HttpGet]
        [Route("getdocumenttypes")]
        public IActionResult GetDocumentTypes()
        {
            return new ApiResult(personInfoService.GetDocumentTypes());
        }

        [HttpPost]
        [Route("GetWordPrintDocument")]
        [GetLogging]
        public IActionResult GetWordPrintDocument([FromBody]GuidIdForm guidIdForm)
        {
            var personInfo = personInfoService.GetPersonInfo(guidIdForm.Id);
            var personInfoModel = personInfoModelBuilder.Build(personInfo);

            var workInfoModelResult = workInfoModelBuilder.Build(personInfo);
            if (!workInfoModelResult.Ok)
                return ApiModelResult.Create(workInfoModelResult);

            if (personInfoModel == null)
            {
                return new ApiResult(new { Message = "Такая карточка не найдена увы" }, 404);
            }

            var res = personInfoWordModelBuilder.BuildTotal(personInfoModel, workInfoModelResult.Data);
            return res;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(PersonInfoForm personInfo)
        {
            var result = personInfoService.Create(personInfo);

            return new ApiResult(new
            {
                PersonRootId = result.RootId
            }, 200);
        }

        [HttpGet]
        [Route("getall")]
        [GetLogging]
        public IActionResult GetAllPersonInfos()
        {
            var personInfos = personInfoService.GetAllPersonInfos();
            var result = personInfoModelBuilder.Build(personInfos);
            return new ApiResult(result);
        }

        [HttpPost]
        [Route("get")]
        [GetLogging]
        public IActionResult GetPersonInfo(GuidIdForm guidIdForm)
        {
            var personInfo = personInfoService.GetPersonInfo(guidIdForm.Id);
            var result = personInfoModelBuilder.Build(personInfo);
            return new ApiResult(result);
        }

        [HttpPost]
        [Route("getarchive")]
        [GetLogging]
        public IActionResult GetArchive(IntDataForm intDataForm)
        {
            var personInfos = personInfoService.GetArchive(intDataForm.IntData);
            var result = personInfoModelBuilder.Build(personInfos);
            return new ApiResult(result);
        }

        [HttpPost]
        [Route("search")]
        [GetLogging]
        public IActionResult SearchPersonInfo(SearchForm searchForm)
        {
            var searchQuery = readDbContext.Get<PersonInfo>().Search(searchForm.FIO, false);
            var pagesCount = searchQuery.Count() / 50 + 1;

            var result = personInfoPreviewModelBuilder.Build(searchQuery, searchForm.PageNumber, readDbContext, !string.IsNullOrWhiteSpace(searchForm.FIO));

            return new ApiResult(new { pagesCount, result });
        }

        [HttpPost]
        [Route("SearchArchive")]
        [GetLogging]
        public IActionResult SearchArchive(SearchForm searchForm)
        {
            var searchQuery = readDbContext.Get<PersonInfo>().Search(searchForm.FIO, true);
            var pagesCount = searchQuery.Count() / 50 + 1;

            var result = personInfoPreviewModelBuilder.Build(searchQuery, searchForm.PageNumber, readDbContext, !string.IsNullOrWhiteSpace(searchForm.FIO));

            return new ApiResult(new { pagesCount, result });
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult UpdateUser(PersonInfoForm personInfo)
        {
            personInfoService.Update(personInfo);
            return new ApiResult(new { Message = "OK" }, 200);
        }

        [HttpGet]
        [Route("getconstantlist")]
        public IActionResult GetConstantList()
        {
            var temp = new PersonInfoConstantList
            {
                EmploymentTypes = EmployeeTypes.employeeTypes,
                PensionTypes = PensionTypes.pensionTypes,
                PayoutTypes = PayoutTypes.payoutTypes,
                PersonDocumentTypes = DocumentTypes.documentTypes,
                AdditionalPensionTypes = AdditionalPensionTypes.additionalPensionTypes,
                Districts = Districts.districts,
                NextNumber = personInfoService.GetNextPersonInfoNumber(),
                DocumentIssuers = DocumentIssuePlaceProvider.GetDocumentIssuePlaces()
            };

            return new ApiResult(temp);
        }

        [HttpPost]
        [Route("snilsrequest")]
        public IActionResult RequestPfrSnils(GuidIdForm form)
        {
            var person = readDbContext.Get<PersonInfo>()
                .ByRootId(form.Id)
                .FirstOrDefault();

            if (person == null)
            {
                return new ApiResult(new
                {
                    message = "карточка не найдена"
                }, 400);
            }

            switch (pfrSnilsService.Enqueue(person))
            {
                case PfrSnilsServiceEnqueueResult.Enqueued:
                    return new ApiResult(new
                    {
                        message = "запрос принят в обработку"
                    });
                case PfrSnilsServiceEnqueueResult.InProcess:
                    return new ApiResult(new
                    {
                        message = "запрос находится в обработке"
                    });
                case PfrSnilsServiceEnqueueResult.Error:
                    return new ApiResult(new
                    {
                        message = "не удалось отправить запрос"
                    }, 500);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}