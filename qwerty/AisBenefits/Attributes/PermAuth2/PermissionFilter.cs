using AisBenefits.ActionResults;
using AisBenefits.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AisBenefits.Attributes.PermAuth2
{
    public class PermissionFilter : Attribute, IAuthorizationFilter
    {
        private readonly string[] permissionList;

        public PermissionFilter(params string[] permissionList)
        {
            this.permissionList = permissionList;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.RequestServices.GetService<ICurrentUserProvider>()
                .GetCurrentUser();

            if(user == null)
            {
                context.Result = new ApiUnauthorizedResult();
                return;
            }

            var usersPermissions = context.HttpContext.RequestServices.GetService<IUsersPermissionGetService>()
                .GetPermissions(user.Id);
            
            if(permissionList.All(p => !usersPermissions.Contains(p)))
            {
                context.Result = new ApiForbiddenResult();
                return;
            }
        }
    }
}
