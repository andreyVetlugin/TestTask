using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Eip;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Models.Pfr;
using AisBenefits.Services.GosPensionUpdates;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("/api/system")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.SuperAdministrate)]
    public class SystemController: Controller
    {
        private readonly IPfrBapForPeriodClientConfigProvider pfrBapForPeriodClientConfigProvider;
        private readonly IEipLogger eipLogger;

        public SystemController(IPfrBapForPeriodClientConfigProvider pfrBapForPeriodClientConfigProvider, IEipLogger eipLogger)
        {
            this.pfrBapForPeriodClientConfigProvider = pfrBapForPeriodClientConfigProvider;
            this.eipLogger = eipLogger;
        }

        [Route("clearpfrsync")]
        public IActionResult ClearPfrSyncRequests()
        {
            var operationResult = GosPensionUpdateSyncService.ClearRequests(pfrBapForPeriodClientConfigProvider.Get(), eipLogger);

            return new ApiOperationResult(operationResult);
        }
    }
}
