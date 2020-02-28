using System;
using System.Linq;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.ExtraPayVariants
{
    using ExtraPayVariantEditModelDataResult = ModelDataResult<ExtraPayVariantEditData>;

    public static class ExtraPayVariantOperationHelper
    {
        public static OperationResult CreateExtraPayVariant(
            this IWriteDbContext<IBenefitsEntity> writeDbContext, ExtraPayVariantEditModelDataResult modelDataResult)
        {
            if(!modelDataResult.Ok)
                return OperationResult.BuildErrorFrom(modelDataResult);

            var newVariant = new ExtraPayVariant
            {
                Id = Guid.NewGuid()
            };

            var form = modelDataResult.Data.Form;

            Mapper.Map(form, newVariant);

            writeDbContext.Add(newVariant);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        public static OperationResult EditExtraPayVariant(
            this IWriteDbContext<IBenefitsEntity> writeDbContext, ExtraPayVariantEditModelDataResult modelDataResult)
        {
            if (!modelDataResult.Ok)
                return OperationResult.BuildErrorFrom(modelDataResult);

            var variant = modelDataResult.Data.VariantModelData.Variant;
            var form = modelDataResult.Data.Form;

            writeDbContext.Attach(variant);
            Mapper.Map(form, variant);
            
            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        public static OperationResult DeleteExtraPayVariant(
            this IWriteDbContext<IBenefitsEntity> writeDbContext, ExtraPayVariantEditModelDataResult modelDataResult)
        {
            if(!modelDataResult.Ok)
                return OperationResult.BuildErrorFrom(modelDataResult);

            var data = modelDataResult.Data.VariantModelData;

            writeDbContext.Remove(data.Variant);
            
            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        public static ExtraPayVariantEditModelDataResult BuildExtraPayVariantEditModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, ServiceHelperEditType editType, IExtraPayVariantEditForm form)
        {
            var modelDataResult = form.VariantId.HasValue && editType.IsForExist()
                ? readDbContext.GetExtraPayVariantModelData(form.VariantId.Value)
                : null;

            if (modelDataResult != null && !modelDataResult.Ok)
                return ExtraPayVariantEditModelDataResult.BuildErrorFrom(modelDataResult);

            if (modelDataResult != null && modelDataResult.Data.HasUsages && editType.IsForExist())
                return ExtraPayVariantEditModelDataResult.BuildInnerStateError("Указанный вариант используется в системе");

            var variantByNumber = !editType.IsDelete()
                ? readDbContext.Get<ExtraPayVariant>()
                    .ByNumber(form.Number)
                    .FirstOrDefault()
                : null;

            if (modelDataResult != null && variantByNumber != null &&
                modelDataResult.Data.Variant.Id != variantByNumber.Id && !editType.IsDelete())
                return ExtraPayVariantEditModelDataResult.BuildFormError("Вариант с таким номером уже существует");

            return ExtraPayVariantEditModelDataResult.BuildSucces(
                new ExtraPayVariantEditData(form, modelDataResult?.Data)
            );
        }
    }

    public class ExtraPayVariantEditData
    {
        public IExtraPayVariantEditForm Form { get; }

        public ExtraPayVariantModelData VariantModelData { get; }

        public ExtraPayVariantEditData(IExtraPayVariantEditForm form, ExtraPayVariantModelData variantModelData)
        {
            Form = form;
            VariantModelData = variantModelData;
        }
    }

    public interface IExtraPayVariantEditForm
    {
        Guid? VariantId { get;}

        int Number { get; }
        decimal? UralMultiplier { get; }
        decimal? PremiumPerc { get; }
        decimal? MatSupportMultiplier { get; }
        decimal? VyslugaMultiplier { get; }
        decimal? VyslugaDivPerc { get; }
    }
}
