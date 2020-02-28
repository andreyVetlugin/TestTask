using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.Organizations;
using AisBenefits.Models.Organizations;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using AisBenefits.Infrastructure.Services.Functions;
using System.Linq;
using AisBenefits.Attributes.Logging;
using AisBenefits.Models.Functions;

namespace AisBenefits.Controllers
{
    [Route("api/functions")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministarateCatalog)]
    public class FunctionController : ControllerBase
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        private readonly IFunctionService functionService;

        public FunctionController(IFunctionService functionService, IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.functionService = functionService;
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
        }

        [HttpPost]
        [Route("create")]
        public void Create(FunctionForm data)
        {
            functionService.Create(data.Name);
        }

        [HttpPost]
        [Route("edit")]
        public void Edit(FunctionForm data)
        {
            functionService.Edit(data.Id.Value, data.Name);
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(FunctionDeleteForm form)
        {
            var modelDataResult = readDbContext.GetFunctionModelData(form.FunctionId);
            var operationResult = writeDbContext.DeleteFunction(modelDataResult);

            return new ApiOperationResult(operationResult);
        }

        [HttpGet]
        [Route("getall")]
        [GetLogging]
        public IActionResult GetAll()
        {
            var result = readDbContext.GetAllFunctionsModelData()
                .Data.Select(f => new
                {
                    f.Function.Id,
                    f.Function.Name,
                    f.HasUsages
                }).OrderBy(c=>c.Name);

            return new ApiResult(result);
        }
    }
}