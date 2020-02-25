using GamesManager.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GamesManager.ActionResults
{
    public class ApiOperationResult : IActionResult
    {
        private readonly IOperationResult operationResult;
        private readonly Func<object> dataProvider;

        public ApiOperationResult(IOperationResult operationResult, Func<object> dataProvider = null)
        {
            this.operationResult = operationResult;
            this.dataProvider = dataProvider;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            if (!operationResult.Ok)
            {
                switch (operationResult.ErrorType)
                {
                    case ServiceHelperErrorType.InvalidForm:
                        return new BadRequestObjectResult(operationResult.ErrorMessage).ExecuteResultAsync(context);
                    case ServiceHelperErrorType.InvalidInnerState:
                        return new ConflictObjectResult(operationResult.ErrorMessage).ExecuteResultAsync(context);
                    default:
                        throw new NotImplementedException();
                }
            }

            operationResult.Complete();

            return (dataProvider == null
                ? new OkResult() as IActionResult
                : new ApiResult(dataProvider()))
                .ExecuteResultAsync(context);
        }
    }
}
