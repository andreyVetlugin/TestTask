using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.KpCodes;
using AisBenefits.Services.EGISSO.KpCodes;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/EgissoKpCode")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePayouts)]
    public class EgissoKpCodeController : ControllerBase
    {

        private readonly IKpCodeEditCreateFormHandler kpCodeEditCreateFormHandler;
        private readonly IKpCodeEditEditFormHandler kpCodeEditEditFormHandler;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public EgissoKpCodeController(IKpCodeEditCreateFormHandler kpCodeEditCreateFormHandler, IKpCodeEditEditFormHandler kpCodeEditEditFormHandler, IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.kpCodeEditCreateFormHandler = kpCodeEditCreateFormHandler;
            this.kpCodeEditEditFormHandler = kpCodeEditEditFormHandler;
            this.readDbContext = readDbContext;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(KpCodeEditForm formHolder)
        {
            var result = kpCodeEditCreateFormHandler.Handle(formHolder, ModelState);

            return new ApiOperationResult(result);
        }


        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            var result = ModelDataResult<List<KpCode>>.BuildSucces(readDbContext.Get<KpCode>().ToList());

            return ApiModelResult.Create(result);
        }




        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(KpCodeEditForm formHolder)
        {
            var result = kpCodeEditEditFormHandler.Handle(formHolder, ModelState);

            return new ApiOperationResult(result);

        }

    }
}