using DataLayer.Infrastructure.DbContexts;
using System;
using System.Linq;

namespace GamesManager.Infrastructure.Services
{
    public enum ServiceHelperEditType
    {
        Create,
        Update,
        Delete
    }

    public static class ServiceHelperEditTypeExtensions
    {
        public static bool IsDelete(this ServiceHelperEditType editType) => editType == ServiceHelperEditType.Delete;
        public static bool IsCreate(this ServiceHelperEditType editType) => editType == ServiceHelperEditType.Create;
        public static bool IsUpdate(this ServiceHelperEditType editType) => editType == ServiceHelperEditType.Update;

        public static bool IsPositive(this ServiceHelperEditType editType) => editType.IsCreate() || editType.IsUpdate();

        public static bool IsForExist(this ServiceHelperEditType editType) => editType != ServiceHelperEditType.Create;
    }

    public enum ServiceHelperErrorType
    {
        None,
        InvalidForm,
        InvalidInnerState
    }

    public interface IChainResult
    {
        bool Ok { get; }
        string ErrorMessage { get; }
        ServiceHelperErrorType ErrorType { get; }
    }

    public interface IModelDataResult : IChainResult
    {
        bool NotExist { get; }
    }

    public class ModelDataResult<TModelData> : IModelDataResult
    {
        public bool Ok => ErrorMessage == null;
        public string ErrorMessage { get; }
        public ServiceHelperErrorType ErrorType { get; }
        public TModelData Data { get; }
        public bool NotExist { get; }

        private ModelDataResult(string errorMessage, ServiceHelperErrorType errorType, TModelData data, bool notExist)
        {
            ErrorMessage = errorMessage;
            ErrorType = errorType;
            Data = data;
            NotExist = notExist;
        }

        public static ModelDataResult<TModelData> BuildSucces(TModelData data)
        {
            return new ModelDataResult<TModelData>(null, ServiceHelperErrorType.None, data, false);
        }

        public static ModelDataResult<TModelData> BuildInnerStateError(string error)
        {
            return new ModelDataResult<TModelData>(error, ServiceHelperErrorType.InvalidInnerState, default(TModelData),
                false);
        }

        public static ModelDataResult<TModelData> BuildFormError(string error)
        {
            return new ModelDataResult<TModelData>(error, ServiceHelperErrorType.InvalidForm, default(TModelData),
                false);
        }

        public static ModelDataResult<TModelData> BuildNotExist(string error)
        {
            return new ModelDataResult<TModelData>(error, ServiceHelperErrorType.InvalidForm, default(TModelData),
                true);
        }

        public static ModelDataResult<TModelData> BuildErrorFrom(IChainResult chainResult)
        {
            return new ModelDataResult<TModelData>(chainResult.ErrorMessage, chainResult.ErrorType,
                default(TModelData), false);
        }
        public static ModelDataResult<TModelData> BuildErrorFrom(IOperationResult operationResult)
        {
            return new ModelDataResult<TModelData>(operationResult.ErrorMessage, operationResult.ErrorType,
                default(TModelData), false);
        }

        public ModelDataResult<TResultData> Then<TResultData>(Func<ModelDataResult<TModelData>, ModelDataResult<TResultData>> selector)
        {
            if (!Ok)
                return ModelDataResult<TResultData>.BuildErrorFrom(this);

            return selector(this);
        }
        public OperationResult<TResultData> Then<TResultData>(Func<ModelDataResult<TModelData>, OperationResult<TResultData>> select)
        {
            if (!Ok)
                return OperationResult<TResultData>.BuildErrorFrom(this);

            return select(this);
        }
        public OperationResult Then(Func<ModelDataResult<TModelData>, OperationResult> select)
        {
            if (!Ok)
                return OperationResult.BuildErrorFrom(this);

            return select(this);
        }
    }

    public interface IOperationResult : IChainResult, IUnitOfWork
    {
    }

    public class OperationResult : IOperationResult
    {
        public bool Ok => ErrorMessage == null;
        public ServiceHelperErrorType ErrorType { get; }
        public string ErrorMessage { get; }

        private readonly IUnitOfWork unitOfWork;

        public static OperationResult BuildSuccess(IUnitOfWork unitOfWork)
        {
            return new OperationResult(unitOfWork, ServiceHelperErrorType.None, null);
        }

        public static OperationResult BuildErrorFrom(IChainResult chainResult)
        {
            return new OperationResult(UnitOfWork.None(), chainResult.ErrorType, chainResult.ErrorMessage);
        }

        public static OperationResult BuildInnerStateError(string error)
        {
            return new OperationResult(UnitOfWork.None(), ServiceHelperErrorType.InvalidInnerState, error);
        }

        public static OperationResult BuildFormError(string error)
        {
            return new OperationResult(UnitOfWork.None(), ServiceHelperErrorType.InvalidForm, error);
        }

        public static OperationResult BuildFromResults(params IOperationResult[] operationResults)
        {
            if (operationResults.All(r => r.Ok))
                return BuildSuccess(UnitOfWork.Complex(operationResults));

            var errorResult = operationResults.First(r => !r.Ok);
            return new OperationResult(errorResult, errorResult.ErrorType, errorResult.ErrorMessage);
        }

        private OperationResult(IUnitOfWork unitOfWork, ServiceHelperErrorType errorType, string errorMessage)
        {
            ErrorType = errorType;
            ErrorMessage = errorMessage;
            this.unitOfWork = unitOfWork;
        }

        public void Complete()
        {
            if (Ok)
                unitOfWork.Complete();
        }
    }

    public class OperationResult<TResultModel> : IOperationResult
    {
        public bool Ok => ErrorMessage == null;
        public ServiceHelperErrorType ErrorType { get; }
        public string ErrorMessage { get; }
        public TResultModel ResultModel { get; }

        private readonly IUnitOfWork unitOfWork;

        public static OperationResult<TResultModel> BuildSuccess(IUnitOfWork unitOfWork, TResultModel resultModel)
        {
            return new OperationResult<TResultModel>(unitOfWork, ServiceHelperErrorType.None, null, resultModel);
        }

        public static OperationResult<TResultModel> BuildErrorFrom(IChainResult chainResult)
        {
            return new OperationResult<TResultModel>(UnitOfWork.None(), chainResult.ErrorType, chainResult.ErrorMessage,
                default(TResultModel));
        }

        public static OperationResult<TResultModel> BuildErrorFrom(IOperationResult modelDataResult)
        {
            return new OperationResult<TResultModel>(UnitOfWork.None(), modelDataResult.ErrorType, modelDataResult.ErrorMessage,
                default(TResultModel));
        }

        public static OperationResult<TResultModel> BuildInnerStateError(string error)
        {
            return new OperationResult<TResultModel>(UnitOfWork.None(), ServiceHelperErrorType.InvalidInnerState, error,
                default(TResultModel));
        }

        public static OperationResult<TResultModel> BuildFormError(string error)
        {
            return new OperationResult<TResultModel>(UnitOfWork.None(), ServiceHelperErrorType.InvalidForm, error, default(TResultModel));
        }

        private OperationResult(IUnitOfWork unitOfWork, ServiceHelperErrorType errorType, string errorMessage, TResultModel resultModel)
        {
            ErrorType = errorType;
            ErrorMessage = errorMessage;
            ResultModel = resultModel;
            this.unitOfWork = unitOfWork;
        }

        public void Complete()
        {
            if (Ok)
                unitOfWork.Complete();
        }
    }

    public interface IUnitOfWork
    {
        void Complete();
    }

    public static class UnitOfWork
    {
        private static readonly NoneOfWork none = new NoneOfWork();

        public static IUnitOfWork None()
        {
            return none;
        }

        public static IUnitOfWork WriteDbContext<TIDbEntity>(IWriteDbContext<TIDbEntity> writeDbContext)
            where TIDbEntity : class
        {
            return new WriteDbContextSaveWork<IWriteDbContext<TIDbEntity>, TIDbEntity>(writeDbContext);
        }

        public static IUnitOfWork Complex(params IUnitOfWork[] unitOfWorks)
        {
            return new ComplexUnitOfWork(unitOfWorks);
        }
    }

    public class NoneOfWork : IUnitOfWork
    {
        public void Complete()
        {
        }
    }

    public class WriteDbContextSaveWork<TWriteDbContext, TIDbEntity> : IUnitOfWork
        where TWriteDbContext : IWriteDbContext<TIDbEntity>
        where TIDbEntity : class
    {
        private readonly TWriteDbContext writeDbContext;

        public WriteDbContextSaveWork(TWriteDbContext writeDbContext)
        {
            this.writeDbContext = writeDbContext;
        }

        public void Complete()
        {
            writeDbContext.SaveChanges();
        }
    }

    public class ComplexUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork[] unitOfWorks;

        public ComplexUnitOfWork(params IUnitOfWork[] unitOfWorks)
        {
            this.unitOfWorks = unitOfWorks;
        }

        public void Complete()
        {
            foreach (var unitOfWork in unitOfWorks)
                unitOfWork.Complete();
        }
    }

    public static class OperationResultExtensions
    {
        public static OperationResult<TResultData> AsOperationResult<TResultData>(this ModelDataResult<TResultData> source, IOperationResult operationResult)
        {
            if (!source.Ok)
                return OperationResult<TResultData>.BuildErrorFrom(source);

            return OperationResult<TResultData>.BuildSuccess(operationResult, source.Data);
        }

        public static OperationResult<TResultData> AttachData<TResultData>(this OperationResult source, TResultData data)
        {
            if (!source.Ok)
                return OperationResult<TResultData>.BuildErrorFrom(source);

            return OperationResult<TResultData>.BuildSuccess(source, data);
        }
        public static OperationResult<TResultData> AttachOperationResult<TResultData>(this OperationResult<TResultData> source, IOperationResult operationResult)
        {
            if (!source.Ok)
                return OperationResult<TResultData>.BuildErrorFrom(source);

            return OperationResult<TResultData>.BuildSuccess(UnitOfWork.Complex(source, operationResult), source.ResultModel);
        }
        public static OperationResult AttachOperationResult(this OperationResult source, IOperationResult operationResult)
        {
            if (!source.Ok)
                return OperationResult.BuildErrorFrom(source);

            return OperationResult.BuildSuccess(UnitOfWork.Complex(source, operationResult));
        }

        public static OperationResult<TResultData> Then<TOperationResult, TResultData>(this TOperationResult source, Func<TOperationResult, OperationResult<TResultData>> select)
            where TOperationResult : IOperationResult
        {
            if (!source.Ok)
                return OperationResult<TResultData>.BuildErrorFrom(source);

            return select(source).AttachOperationResult(source);
        }

        public static OperationResult Then<TOperationResult>(this TOperationResult source, Func<TOperationResult, OperationResult> select)
            where TOperationResult : IOperationResult
        {
            if (!source.Ok)
                return OperationResult.BuildErrorFrom(source);

            return select(source).AttachOperationResult(source);
        }
    }
}
