using System;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.Logging;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Eip;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.GosPensions;
using AisBenefits.Models;
using AisBenefits.Models.GosPensions;
using AisBenefits.Models.Pfr;
using AisBenefits.Services;
using AisBenefits.Services.GosPensionUpdates;
using AisBenefits.Services.Pensions;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AisBenefits.Controllers
{
    [Route("api/GosPensionUpdate")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePersonInfos)]
    public class GosPensionUpdateController : ControllerBase
    {
        private readonly BenefitsAppContext bfsContext;
        private readonly PfrGosPensionContext pfrContext;

        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IPfrBapForPeriodClientConfigProvider pfrBapForPeriodClientConfigProvider;
        private readonly IEipLogger eipLogger;
        private readonly IConfiguration configuration;

        private readonly IGosPensionUpdateService gosPensionUpdateService;
        private readonly IGosPensionUpdateModelBuilder gosPensionUpdateModelBuilder;

        public GosPensionUpdateController(BenefitsAppContext bfsContext, PfrGosPensionContext pfrContext, IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext, IPfrBapForPeriodClientConfigProvider pfrBapForPeriodClientConfigProvider, IEipLogger eipLogger, IConfiguration configuration, IGosPensionUpdateService gosPensionUpdateService, IGosPensionUpdateModelBuilder gosPensionUpdateModelBuilder)
        {
            this.bfsContext = bfsContext;
            this.pfrContext = pfrContext;
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
            this.pfrBapForPeriodClientConfigProvider = pfrBapForPeriodClientConfigProvider;
            this.eipLogger = eipLogger;
            this.configuration = configuration;
            this.gosPensionUpdateService = gosPensionUpdateService;
            this.gosPensionUpdateModelBuilder = gosPensionUpdateModelBuilder;
        }

        [HttpPost]
        [Route("get")]
        [GetLogging]
        public IActionResult Get()
        {
            var incomePens = gosPensionUpdateService.GetIncomePensions();
            var result = gosPensionUpdateModelBuilder.Build(incomePens);
            return new ApiResult(result);
        }

        [HttpPost]
        [Route("approve")]
        public IActionResult Approve(AcceptedPensionUpdateForm acceptedPensionUpdateDto)
        {
            var ids = acceptedPensionUpdateDto.GosPensionUpdateIds;
            if (ids.Length<1)
            {
                return new ApiResult(new { Message = "Ничего не выбрано" },404);
            }
            var operationResult = gosPensionUpdateService.ApproveMany(acceptedPensionUpdateDto);
            return new ApiOperationResult(operationResult);
        }

        [HttpPost]
        [Route("approveOne")]
        public IActionResult ApproveOne(AcceptedPensionUpdateFormOne acceptedPension)
        {
            var id = acceptedPension.GosPensionUpdateId;
            if (id==default(Guid))
            {
                return new ApiResult(new { Message = "Ничего не выбрано" }, 404);
            }
            var operationReuslt = gosPensionUpdateService.ApproveOne(acceptedPension.GosPensionUpdateId, 
                                               acceptedPension.Destination, 
                                               acceptedPension.Execution, 
                                               acceptedPension.Comment);

            return new ApiOperationResult(operationReuslt);
        }

        [HttpPost]
        [Route("decline")]
        public IActionResult Decline(DeclinedPensionsArray declinedPensionsArray)
        {
            var updts = declinedPensionsArray.DeclinedPensionUpdates;
            if (updts.Length < 1)
            {
                return new ApiResult(new { Message = "Ничего не выбрано" }, 404);
            }
            gosPensionUpdateService.Decline(updts);
            return new ApiResult(new { Message = "OK" });
        }

        [HttpPost]
        [Route("declineOne")]
        public IActionResult DeclineOne(DeclinedPensionUpdateForm declinedPensionUpdateForm)
        {
            
            if (declinedPensionUpdateForm==null)
            {
                return new ApiResult(new { Message = "Данных нет" }, 404);
            }
            gosPensionUpdateService.DeclineOne(declinedPensionUpdateForm);
            return new ApiResult(new { Message = "OK" });
        }

        [HttpPost, Route("syncpfr")]
        public IActionResult SyncPfr(GuidIdForm form)
        {
            var config = pfrBapForPeriodClientConfigProvider.Get();
            
            var result = bfsContext.SyncGosPension(pfrContext, form.Id);

            return new ApiOperationResult(result);
        }

        [HttpPost, Route("syncpfrall")]
        public IActionResult SyncPfrAll()
        {
            var config = pfrBapForPeriodClientConfigProvider.Get();

            var operationResult = pfrContext.SyncAllGosPensions();

            return new ApiOperationResult(operationResult);
        }
    }
}