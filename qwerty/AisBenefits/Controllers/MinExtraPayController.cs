using AisBenefits.Attributes;
using AisBenefits.Attributes.Logging;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.MinExtraPays;
using AisBenefits.Models.MinExtraPays;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Controllers
{
    [Route("api/[controller]")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministarateCatalog)]
    public class MinExtraPayController : ControllerBase
    {
        private readonly IMinExtraPayService extraPayService;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        public MinExtraPayController(IMinExtraPayService extraPayService, IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.extraPayService = extraPayService;
            this.writeDbContext = writeDbContext;
        }

        [HttpPost]
        [Route("edit")]
        public void Edit(MinExtraPayEditForm extraPayEditForm)
        {
            extraPayService.Edit(writeDbContext, extraPayEditForm);
            writeDbContext.SaveChanges();
        }


        [HttpGet]
        [Route("get")]
        [GetLogging]
        public object Get()
        {
            var minExtraPay = extraPayService.Get();
            return new { minExtraPay.Value };
        }

    }
}