using System;
using System.Threading.Tasks;
using GamesManager.Infrasctructure.Services
using Microsoft.AspNetCore.Mvc;

namespace GamesManager.ActionResults
{
    public static class ApiModelResult
    {
        public static ApiModelResult<TResult> Create<TResult>(ModelDataResult<TResult> modelDataResult, Func<TResult, object> selector = null) =>
            new ApiModelResult<TResult>(modelDataResult, selector);
    }

    public class ApiModelResult<TResult> : IActionResult
    {
        private readonly ModelDataResult<TResult> modelDataResult;
        private readonly Func<TResult, object> selector;

        public ApiModelResult(ModelDataResult<TResult> modelDataResult, Func<TResult, object> selector = null)
        {
            this.modelDataResult = modelDataResult;
            this.selector = selector;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            if (!modelDataResult.Ok)
            {
                switch (modelDataResult.ErrorType)
                {
                    case ServiceHelperErrorType.InvalidForm:
                        return new BadRequestObjectResult(modelDataResult.ErrorMessage).ExecuteResultAsync(context);
                    case ServiceHelperErrorType.InvalidInnerState:
                        return new ConflictObjectResult(modelDataResult.ErrorMessage).ExecuteResultAsync(context);
                    default:
                        throw new NotImplementedException();
                }
            }

            return (selector != null
                   ? new ApiResult(selector(modelDataResult.Data))
                   : new ApiResult(modelDataResult.Data))
                .ExecuteResultAsync(context);
        }
    }
}
