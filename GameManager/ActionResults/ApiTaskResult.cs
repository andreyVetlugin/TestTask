using GamesManager.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GamesManager.ActionResults
{
    public class ApiTaskResult : IActionResult
    {
        private readonly IApiTask task;

        private ApiTaskResult(IApiTask task)
        {
            this.task = task;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            try
            {
                task.Complete();

                return new OkResult().ExecuteResultAsync(context);
            }
            catch (AggregateException exception)
            {
                foreach (var initialException in exception.Flatten().InnerExceptions)
                    switch (initialException)
                    {
                        case InvalidFormException e:
                            return new BadRequestObjectResult(e.Message).ExecuteResultAsync(context);
                        case InvalidInnerStateException e:
                            return new ConflictObjectResult(e.Message).ExecuteResultAsync(context);
                        case AggregateException e:
                            continue;
                        default:
                            return ApiExceptionResult.BuildLocal(initialException).ExecuteResultAsync(context);
                    }
                return ApiExceptionResult.BuildLocal(exception).ExecuteResultAsync(context);
            }
        }

        public static ApiTaskResult Create<TData>(Task<TData> task) =>
            new ApiTaskResult(new ApiTask<TData>(task));

        public static ApiTaskResult Create(Task task) =>
            new ApiTaskResult(new ApiTask(task));

        interface IApiTask
        {
            void Complete();
            IActionResult Excecute();
        }

        class ApiTask : IApiTask
        {
            private readonly Task task;

            public ApiTask(Task task)
            {
                this.task = task;
            }

            public void Complete()
            {
                task.Wait();
            }

            public IActionResult Excecute()
            {
                return new OkResult();
            }
        }
        class ApiTask<TData> : IApiTask
        {
            private readonly Task<TData> task;

            public ApiTask(Task<TData> task)
            {
                this.task = task;
            }

            public void Complete()
            {
                task.Wait();
            }

            public IActionResult Excecute()
            {
                return new ApiResult(task.Result);
            }
        }
    }
}
