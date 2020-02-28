using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Infrastructure.Services.PostInfoLogs;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;

namespace AisBenefits.Infrastructure.Services.ExtraPays
{
    using ExtraPayEditDataResult = ModelDataResult<ExtraPayEditData>;
    using ExtraPayModelDataResult = ModelDataResult<ExtraPayModelData>;

    public static class ExtraPayOperationHelper
    {

        public static ExtraPayEditDataResult GetEditExtraPayData(this IReadDbContext<IBenefitsEntity> readDbContext,
            IExtraPayEditForm form, ExtraPayRecalculateType recalculateType, User user,
            ExtraPayModelDataResult modelDataResult)
        {
            if (!modelDataResult.Ok)
                return ExtraPayEditDataResult.BuildErrorFrom(modelDataResult);

            var data = modelDataResult.Data;

            return readDbContext.GetEditExtraPayData(form, recalculateType, user, data);
        }

        public static ExtraPayEditDataResult GetEditExtraPayData(this IReadDbContext<IBenefitsEntity> readDbContext, IExtraPayEditForm form, ExtraPayRecalculateType recalculateType, User user, ExtraPayModelData extraPayModelData)
        {
            var data = extraPayModelData;

            if (form.PersonRootId != data.Instance.PersonRootId)
                throw new InvalidOperationException();

            if(data.WorkAgeDays == 0 && data.Initial)
                return ExtraPayEditDataResult.BuildInnerStateError("Раздел стаж не заполнен");

            var newExtraPay = new ExtraPay
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now
            };

            Mapper.Map(form, newExtraPay);

            if(recalculateType == ExtraPayRecalculateType.Default && form.TotalExtraPay < extraPayModelData.MinExtraPay.Value)
                return ExtraPayEditDataResult.BuildFormError("Доплата не может быть меньше минимальной доплаты");

            newExtraPay.DsPerc = data.DsPerc?.Amount ?? 0;

            var operationType = recalculateType == ExtraPayRecalculateType.Default
                ? PostOperationType.ExtraPayEdit
                : PostOperationType.ExtraPayRecalculate;

            var operation = PostInfoLogHelper.Create(operationType, form.PersonRootId, user.Id);

            var result = new ExtraPayEditData(data, newExtraPay, operation, recalculateType);

            return ExtraPayEditDataResult.BuildSucces(result);
        }

        public static OperationResult EditExtraPay(this IWriteDbContext<IBenefitsEntity> writeDbContext, ExtraPayEditDataResult dataResult)
        {
            if (!dataResult.Ok)
                return OperationResult.BuildErrorFrom(dataResult);

            var data = dataResult.Data;

            if (data.Type != ExtraPayEditType.Edit)
                throw new InvalidOperationException();

            var extraPay = data.ModelData;

            if (!extraPay.Initial)
            {
                var instance = extraPay.Instance;
                writeDbContext.Attach(instance);
                instance.OutDate = DateTime.Now;
            }

            var newExtraPay = data.NewExtraPay;

            writeDbContext.Add(newExtraPay);

            var operation = data.Operation;
            writeDbContext.Add(operation);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        public static OperationResult<ExtraPay> RecalculateExtraPay(this IWriteDbContext<IBenefitsEntity> writeDbContext, ExtraPayEditDataResult modelDataResult)
        {
            RecalculateWithoutSave(modelDataResult);

            var data = modelDataResult.Data;
            var extraPay = data.ModelData.Instance;
            var newExtraPay = data.NewExtraPay;

            writeDbContext.Attach(extraPay);
            extraPay.OutDate = newExtraPay.CreateDate;
            writeDbContext.Add(newExtraPay);
            writeDbContext.Add(data.Operation);

            return OperationResult<ExtraPay>.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext), newExtraPay);
        }


        public static ModelDataResult<ExtraPay> RecalculateWithoutSave(ExtraPayEditDataResult modelDataResult)
        {
            if (!modelDataResult.Ok)
                return ModelDataResult<ExtraPay>.BuildErrorFrom(modelDataResult);

            var data = modelDataResult.Data;

            if (data.Type != ExtraPayEditType.Recalculate)
                throw new InvalidOperationException();

            var extraPay = data.ModelData;

            if (extraPay.Initial)
                return ModelDataResult<ExtraPay>.BuildFormError("Невозможно пересчитать доплату. Данные не заполнены.");

            var newExtraPay = data.NewExtraPay;

            // newExtraPay.XX = newExtraPay.XX
            // означает что принимаются новые значения, на основе которых будет производиться пересчет
            var oldExtraPay = extraPay.Instance;
            var variant = data.ModelData.Variant;

            var rcSalary = data.RecalculateType.HasFlag(ExtraPayRecalculateType.Salary);

            newExtraPay.VariantId = oldExtraPay.VariantId;
            newExtraPay.UralMultiplier = oldExtraPay.UralMultiplier;

            newExtraPay.Salary = rcSalary
                ? newExtraPay.Salary
                : oldExtraPay.Salary;

            var salaryMultiplier = newExtraPay.Salary / oldExtraPay.Salary;

            newExtraPay.Premium = oldExtraPay.Premium * salaryMultiplier;

            newExtraPay.MaterialSupport = oldExtraPay.MaterialSupport * salaryMultiplier;

            newExtraPay.Perks = oldExtraPay.Perks * salaryMultiplier;

            newExtraPay.Vysluga = oldExtraPay.Vysluga * salaryMultiplier;
            newExtraPay.Secrecy = oldExtraPay.Secrecy * salaryMultiplier;
            newExtraPay.Qualification = oldExtraPay.Qualification * salaryMultiplier;

            var salaryMultiplied = newExtraPay.Salary * newExtraPay.UralMultiplier;

            newExtraPay.Ds = rcSalary
                ? salaryMultiplied + newExtraPay.Premium + newExtraPay.MaterialSupport + newExtraPay.Perks + newExtraPay.Vysluga + newExtraPay.Secrecy + newExtraPay.Qualification
                : oldExtraPay.Ds;

            newExtraPay.DsPerc = data.RecalculateType.HasFlag(ExtraPayRecalculateType.WorkAge)
                ? extraPay.DsPerc?.Amount ?? 0
                : oldExtraPay.DsPerc;
            newExtraPay.GosPension = data.RecalculateType.HasFlag(ExtraPayRecalculateType.GosPension)
                ? newExtraPay.GosPension
                : oldExtraPay.GosPension;
            newExtraPay.ExtraPension = data.RecalculateType.HasFlag(ExtraPayRecalculateType.ExtraPension)
                ? newExtraPay.ExtraPension
                : oldExtraPay.ExtraPension;
            newExtraPay.TotalPension = newExtraPay.GosPension + newExtraPay.ExtraPension;
            newExtraPay.TotalPensionAndExtraPay = newExtraPay.Ds * newExtraPay.DsPerc / 100;
            newExtraPay.TotalExtraPay =
                Math.Max(
                    extraPay.MinExtraPay.Value,
                    newExtraPay.TotalPensionAndExtraPay - newExtraPay.TotalPension + (extraPay.Variant.IgnoreGosPension
                    ? newExtraPay.GosPension : 0)
                    );
           

            return ModelDataResult<ExtraPay>.BuildSucces(newExtraPay);
        }


    }

    public class ExtraPayEditData
    {
        public ExtraPayModelData ModelData { get; }
        public ExtraPay NewExtraPay { get; }
        public PostInfoLog Operation { get; }

        public ExtraPayEditType Type => RecalculateType == ExtraPayRecalculateType.Default
            ? ExtraPayEditType.Edit
            : ExtraPayEditType.Recalculate;
        public ExtraPayRecalculateType RecalculateType { get; }

        public ExtraPayEditData(ExtraPayModelData modelData, ExtraPay newExtraPay, PostInfoLog operation, ExtraPayRecalculateType recalculateType)
        {
            ModelData = modelData;
            NewExtraPay = newExtraPay;
            Operation = operation;
            RecalculateType = recalculateType;
        }
    }

    public enum ExtraPayEditType
    {
        Edit,
        Recalculate
    }

    public class ExtraPayEditResultData
    {
        public ExtraPayEditData EditData { get; }

        public ExtraPay ResultInstance => EditData.NewExtraPay;

        public ExtraPayEditResultData(ExtraPayEditData editData)
        {
            EditData = editData;
        }
    }

    public interface IExtraPayEditForm
    {
        Guid PersonRootId { get; }
        Guid VariantId { get; }

        decimal UralMultiplier { get; }
        decimal Salary { get; }
        decimal Premium { get; }
        decimal MaterialSupport { get; }
        decimal Perks { get; }
        decimal Vysluga { get; }
        decimal Secrecy { get; }
        decimal Qualification { get; }
        decimal GosPension { get; }
        decimal ExtraPension { get; }
        decimal Ds { get; }
        decimal TotalExtraPay { get; }
        decimal TotalPension { get; }
        decimal TotalPensionAndExtraPay { get; }
    }

    [Flags]
    public enum ExtraPayRecalculateType
    {
        Default,

        GosPension = 1,
        Salary = 2,
        WorkAge = 4,
        ExtraPension = 8
    }

    public class ExtraPayRecalculateForm : IExtraPayEditForm
    {
        public Guid PersonRootId { get; }
        public Guid VariantId { get; private set; }
        public decimal UralMultiplier { get; private set; }
        public decimal Salary { get; private set; }
        public decimal Premium { get; private set; }
        public decimal MaterialSupport { get; private set; }
        public decimal Perks { get; private set; }
        public decimal Vysluga { get; private set; }
        public decimal Secrecy { get; private set; }
        public decimal Qualification { get; private set; }
        public decimal GosPension { get; private set; }
        public decimal ExtraPension { get; private set; }
        public decimal Ds { get; private set; }
        public decimal TotalExtraPay { get; private set; }
        public decimal TotalPension { get; private set; }
        public decimal TotalPensionAndExtraPay { get; private set; }

        public static IExtraPayEditForm CreateGosPension(Guid personRootId, decimal gosPension)
        {
            return new ExtraPayRecalculateForm(personRootId)
            {
                GosPension = gosPension
            };
        }
        public static IExtraPayEditForm CreateSalary(Guid personRootId, decimal salary)
        {
            return new ExtraPayRecalculateForm(personRootId)
            {
                Salary = salary
            };
        }
        public static IExtraPayEditForm CreateExtraPensionAndGosPension(Guid personRootId, decimal extraPension, decimal gosPension)
        {
            return new ExtraPayRecalculateForm(personRootId)
            {
                GosPension = gosPension,
                ExtraPension = extraPension
            };
        }
        public static IExtraPayEditForm CreateDsPerc(Guid personRootId)
        {
            return new ExtraPayRecalculateForm(personRootId)
            {
            };
        }

        public ExtraPayRecalculateForm(Guid personRootId)
        {
            PersonRootId = personRootId;
        }

    }
}
