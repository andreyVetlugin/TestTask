using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace GamesManager.ActionResults
{
    public class ApiResult : IActionResult
    {
        private readonly JsonResult jsonResult;

        public ApiResult(object result, int statusCode = 200)
        {
            jsonResult = new JsonResult(result, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            })
            {
                StatusCode = statusCode
            };
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return jsonResult.ExecuteResultAsync(context);
        }
    }
}
