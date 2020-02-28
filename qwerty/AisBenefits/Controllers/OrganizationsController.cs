using System.Linq;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.Logging;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.Organizations;
using AisBenefits.Models;
using AisBenefits.Models.Organizations;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/organizations")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministarateCatalog)]
    public class OrganizationsController : ControllerBase
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        private readonly IOrganizationService organizationService;

        public OrganizationsController(IOrganizationService organizationService, IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.organizationService = organizationService;
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
        }

        [HttpPost]
        [Route("create")]
        public void Create(OrganizationForm data)
        {
            organizationService.Create(data.Name, data.Multiplier);
        }
        [HttpPost]
        [Route("edit")]
        public void Edit(OrganizationForm data)
        {
            organizationService.Edit(data.Id.Value, data.Name, data.Multiplier);
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(GuidIdForm form)
        {
            var modelDataResult = readDbContext.GetOrganizationModelDataResult(form.Id);
            var operationResult = writeDbContext.DeleteOrganization(modelDataResult);

            return new ApiOperationResult(operationResult);
        }

        [HttpGet]
        [Route("getall")]
        [GetLogging]
        public IActionResult GetAll()
        {
            var modelDataResult = readDbContext.GetAllOrganizationsModelDataResult();
            
            return new ApiResult(
                modelDataResult.Data.Select(o => new
                {
                    o.Organization.Id,
                    o.Organization.Multiplier,
                    o.Organization.OrganizationName,
                    o.HasUsages
                }).OrderBy(c=>c.OrganizationName));
        }
    }
}