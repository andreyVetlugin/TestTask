using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisBenefits.Infrastructure.Services.Authorize
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager 

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            
            //TODO: ваш код проверки, есть ли у пользователя права на эти операции            
            if (requirement.Permissions.Any())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
