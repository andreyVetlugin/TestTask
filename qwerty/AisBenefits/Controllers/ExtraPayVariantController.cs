using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AisBenefits.ActionResults;
using AisBenefits.Attributes.Logging;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.ExtraPayVariants;
using AisBenefits.Models;
using AisBenefits.Models.ExtraPayVariants;

namespace AisBenefits.Controllers
{
    [Route("/api/extrapayvariant")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministarateCatalog)]
    public class ExtraPayVariantController : Controller
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        public ExtraPayVariantController(IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
        }

        [Route("getall")]
        [GetLogging]
        public IActionResult GetAll()
        {
            var modelDataResult = readDbContext.GetAllExtraPayVariantsModelData();

            return new ApiResult(
                modelDataResult.Data.Select(v => new
                {
                    v.Variant.Id,
                    v.Variant.MatSupportMultiplier,
                    v.Variant.Number,
                    v.Variant.PremiumPerc,
                    v.Variant.UralMultiplier,
                    v.Variant.VyslugaDivPerc,
                    v.Variant.VyslugaMultiplier,
                    v.Variant.IgnoreGosPension,

                    Editable = !v.HasUsages
                }));
        }

        [Route("create")]
        public IActionResult Create(ExtraPayVariantEditForm form)
        {
            var modelDataResult = readDbContext.BuildExtraPayVariantEditModelData(ServiceHelperEditType.Create, form);
            var operationResult = writeDbContext.CreateExtraPayVariant(modelDataResult);

            return new ApiOperationResult(operationResult);
        }

        [Route("edit")]
        public IActionResult Edit(ExtraPayVariantEditForm form)
        {
            var modelDataResult = readDbContext.BuildExtraPayVariantEditModelData(ServiceHelperEditType.Update, form);
            var operationResult = writeDbContext.EditExtraPayVariant(modelDataResult);

            return new ApiOperationResult(operationResult);
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(GuidIdForm form)
        {
            var modelDataResult = readDbContext.BuildExtraPayVariantEditModelData(
                ServiceHelperEditType.Delete,
                new ExtraPayVariantEditForm
                {
                    VariantId = form.Id
                });
            var operationResult = writeDbContext.DeleteExtraPayVariant(modelDataResult);

            return new ApiOperationResult(operationResult);
        }
    }
}
