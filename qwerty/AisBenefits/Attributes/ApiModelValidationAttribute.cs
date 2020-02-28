using AisBenefits.ActionResults;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace AisBenefits.Attributes
{
    public class ApiModelValidationAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ApiInvalidModelResult(context.ModelState);                
            }
        }
    }
}
