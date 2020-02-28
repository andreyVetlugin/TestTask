using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.Logging;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.ExtraPays;
using AisBenefits.Infrastructure.Services.PersonInfos;
using AisBenefits.Infrastructure.Services.Solutions;
using AisBenefits.Models.ExtraPays;
using AisBenefits.Models.Solutions;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/extrapay")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePayouts)]
    public class ExtraPayController : Controller
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly ICurrentUserProvider currentUserProvider;
        private readonly IPersonInfoService personInfoService;

        public ExtraPayController(IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext, ICurrentUserProvider currentUserProvider, IPersonInfoService personInfoService)
        {
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
            this.currentUserProvider = currentUserProvider;
            this.personInfoService = personInfoService;
        }

        [Route("get")]
        [HttpPost]
        [GetLogging]
        public IActionResult Get(ExtraPayGetForm form)
        {
            var result = readDbContext.GetExtraPayModelData(form.PersonRootId);

            return ApiModelResult.Create(result, d => d.BuildModel());
        }

        [Route("edit")]
        [HttpPost]
        public IActionResult Edit(ExtraPayEditForm form)
        {
            var user = currentUserProvider.GetCurrentUser();

            var editResult = readDbContext.GetExtraPayModelData(form.PersonRootId)
                .Then(r => readDbContext.GetEditExtraPayData(form, ExtraPayRecalculateType.Default, user, r))
                .Then(r => writeDbContext.EditExtraPay(r).AttachData(r.Data));

            var result = !form.CreateSolution
                ? OperationResult.BuildFromResults(editResult)
                : editResult
                    .Then(r => readDbContext.GetSolutionModelData(r.ResultModel.NewExtraPay).AsOperationResult(r))
                    .Then(r =>
                        readDbContext.BuildSolutionEditPositiveModelData(new SolutionForm
                        {
                            PersonInfoRootId = form.PersonRootId,
                            Destination = form.DestinationDate,
                            Execution = form.ExecutionDate,
                            Comment = form.Comment
                        },
                            r.ResultModel
                        ).AsOperationResult(r))
                    .Then(r => writeDbContext.CreateSolution(r.ResultModel).AttachData(r.ResultModel))
                    .Then(r =>
                    {
                        return OperationResult.BuildSuccess(UnitOfWork.None())
                            .AttachOperationResult(r);
                    });

            return new ApiOperationResult(result);
        }
    }
}
