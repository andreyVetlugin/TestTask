using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.ProvisionForms;
using AisBenefits.Services.EGISSO.ProvisionForms;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/EgissoProvisionForm")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePayouts)]
    public class EgissoProvisionFormController : ControllerBase
    {

        private readonly IProvisionFormEditCreateFormHandler provisionFormEditCreateFormHandler;
        private readonly IProvisionFormEditEditFormHandler provisionFormEditEditFormHandler;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public EgissoProvisionFormController(IProvisionFormEditCreateFormHandler provisionFormEditCreateFormHandler, IProvisionFormEditEditFormHandler provisionFormEditEditFormHandler, IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.provisionFormEditCreateFormHandler = provisionFormEditCreateFormHandler;
            this.provisionFormEditEditFormHandler = provisionFormEditEditFormHandler;
            this.readDbContext = readDbContext;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(ProvisionFormEditForm form)
        {
            var result = provisionFormEditCreateFormHandler.Handle(form, ModelState);
            return new ApiOperationResult(result);
        }


        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            var result = ModelDataResult<List<ProvisionForm>>.BuildSucces(readDbContext.Get<ProvisionForm>().ToList());

            return ApiModelResult.Create(result);
        }


        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(ProvisionFormEditForm form)
        {
            var result = provisionFormEditEditFormHandler.Handle(form, ModelState);
            return new ApiOperationResult(result);
        }


    }
}