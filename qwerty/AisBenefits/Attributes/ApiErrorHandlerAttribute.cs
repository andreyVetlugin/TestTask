using AisBenefits.ActionResults;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System;
using System.Net;
using Microsoft.AspNetCore.Http.Internal;

namespace AisBenefits.Attributes
{
    public class ApiErrorHandlerAttribute: ExceptionFilterAttribute
    {
        public ApiErrorHandlerAttribute()
        {
            Order = 1000;
        }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            var errorId = Guid.NewGuid().ToString().Replace("-", "");
            context.Result = new ApiExceptionResult(errorId, exception,
                IPAddress.IsLoopback(context.HttpContext.Connection.RemoteIpAddress));

            context.HttpContext.Request.EnableRewind();
            context.HttpContext.Request.Body.Position = 0;

            context.HttpContext.RequestServices.GetService<ILogger<ApiErrorHandlerAttribute>>()
                .LogError(new EventId(), $"{errorId}\r\n{exception.Message}\r\nEXCEPTION={exception.GetType().Name} {exception.StackTrace}\r\nURL={context.HttpContext.Request.Path}\r\nHEADERS=\r\n{string.Join("\r\n", context.HttpContext.Request.Headers.Select(p => $"\t{p.Key}: {p.Value}"))}\r\nBODY={new StreamReader(context.HttpContext.Request.Body).ReadToEnd()}\r\n");

            context.ExceptionHandled = true;
        }
    }
}
