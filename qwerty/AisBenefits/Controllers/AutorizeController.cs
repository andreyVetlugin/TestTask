using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.Authorize;
using AisBenefits.Models.Authorize;
using AisBenefits.Services.Authorize;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AisBenefits.Controllers
{
    [Route("/api/authorize")]
    [ApiController, ApiErrorHandler]
    [ApiModelValidation]
    public class AutorizeController : Controller
    {
        private readonly IAutorizeService autorizeService;
        private readonly IAuthorizeTokenService authorizeTokenService;

        public AutorizeController(IAutorizeService autorizeService, IAuthorizeTokenService authorizeTokenService)
        {
            this.autorizeService = autorizeService;
            this.authorizeTokenService = authorizeTokenService;
        }

        [HttpPost, Route("auth")]
        public ApiResult Autorize(AuthForm form)
        {
            var result =  autorizeService.Authorize(form.Login, form.Password);
            switch (result)
            {
                case AutorizeServiceSuccessResult r:
                    var userToken = authorizeTokenService.CreateToken(r.User);
                    var permissions = r.Roles
                        .SelectMany(p => RolePermissionsHelper.Resolve(p.Permissions))
                        .ToHashSet();

                    HttpContext.Response.Cookies.Append("Authorization", userToken);

                    return new ApiResult(new
                    {
                        UserId = r.User.Id,
                        Token = userToken,
                        Permissions = permissions
                    });
                case AutorizeServiceErrorResult r:
                    return new ApiResult(new
                    {
                        r.Error
                    }, 403);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
