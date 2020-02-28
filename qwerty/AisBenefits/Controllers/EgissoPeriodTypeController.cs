using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.PeriodTypes;
using AisBenefits.Services.EGISSO.PeriodTypes;
using AisBenefits.Services.EGISSO.PeriodTypes.Create;
using AisBenefits.Services.EGISSO.PeriodTypes.Edit;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/EgissoPeriodType")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePayouts)]
    public class EgissoPeriodTypeController : ControllerBase
    {
        private readonly IPeriodTypeEditCreateFormHandler periodTypeEditCreateFormHandler;
        private readonly IPeriodTypeEditEditFormHandler periodTypeEditEditFormHandler;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public EgissoPeriodTypeController(IPeriodTypeEditCreateFormHandler periodTypeEditCreateFormHandler, IPeriodTypeEditEditFormHandler periodTypeEditEditFormHandler, IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.periodTypeEditCreateFormHandler = periodTypeEditCreateFormHandler;
            this.periodTypeEditEditFormHandler = periodTypeEditEditFormHandler;
            this.readDbContext = readDbContext;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(PeriodTypeEditForm form)
        {
            var result = periodTypeEditCreateFormHandler.Handle(form, ModelState);
            
                return new ApiOperationResult(result);
            
        }


        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            var result = ModelDataResult<List<PeriodType>>.BuildSucces(readDbContext.Get<PeriodType>().ToList());

            return ApiModelResult.Create(result);
        }


        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(PeriodTypeEditForm form)
        {
            var result = periodTypeEditEditFormHandler.Handle(form, ModelState);

            return new ApiOperationResult(result);


        }

    }
}