using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace AisBenefits.ActionResults
{
    public class ApiInvalidModelResult : ApiResult
    {
        public ApiInvalidModelResult(ModelStateDictionary modelState)
            : base(BuildResult(modelState), 400)
        {
        }

        static object BuildResult(ModelStateDictionary modelState)
        {
            return modelState
                .Where(s => s.Value.Errors.Any())
                .ToDictionary(s => s.Key, s => string.Join("; ", s.Value.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
