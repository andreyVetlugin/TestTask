using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.MeasureUnits;
using AisBenefits.Services.EGISSO.MeasureUnits;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/EgissoMeasureUnit")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePayouts)]
    public class EgissoMeasureUnitController : ControllerBase
    {
        private readonly IMeasureUnitEditCreateFormHandler measureUnitEditCreateFormHandler;
        private readonly IMeasureUnitEditEditFormHandler measureUnitEditEditFormHandler;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public EgissoMeasureUnitController(IMeasureUnitEditCreateFormHandler measureUnitEditCreateFormHandler, IMeasureUnitEditEditFormHandler measureUnitEditEditFormHandler, IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.measureUnitEditCreateFormHandler = measureUnitEditCreateFormHandler;
            this.measureUnitEditEditFormHandler = measureUnitEditEditFormHandler;
            this.readDbContext = readDbContext;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(MeasureUnitEditForm form)
        {
            var result = measureUnitEditCreateFormHandler.Handle(form, ModelState);
            return new ApiOperationResult(result);
        }


        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            var result = ModelDataResult<List<MeasureUnit>>.BuildSucces(readDbContext.Get<MeasureUnit>().ToList());

            return ApiModelResult.Create(result);
        }



        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(MeasureUnitEditForm form)
        {
            var result = measureUnitEditEditFormHandler.Handle(form, ModelState);

            return new ApiOperationResult(result);
        }

    }
}