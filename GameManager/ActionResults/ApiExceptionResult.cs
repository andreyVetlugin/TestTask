using System;

namespace GamesManager.ActionResults
{
    public class ApiExceptionResult : ApiResult
    {
        public ApiExceptionResult(string errorId, Exception exception, bool isLocal)
            : base(BuildResult(errorId, exception, isLocal), 500)
        {
        }

        static object BuildResult(string errorId, Exception exception, bool isLocal)
        {
            if (isLocal)
                return new
                {
                    message = "Внутренняя ошибка сервера",
                    exceptionMessage = exception.Message,
                    errorId,
                    exception = exception.GetType().FullName,
                    stacktrace = exception.StackTrace
                };
            else
                return new
                {
                    message = "Внутренняя ошибка сервера",
                    errorId
                };
        }

        public static ApiExceptionResult BuildLocal(Exception exception)
        {
            return new ApiExceptionResult(Guid.NewGuid().ToString().Replace("-", ""), exception, true);
        }
    }
}
