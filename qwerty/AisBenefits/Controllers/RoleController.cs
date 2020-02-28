using System.Linq;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.UsersAndRoles;
using AisBenefits.Models;
using AisBenefits.Models.UsersAndRoles.INPUT;
using AisBenefits.Services.UsersAndRoles;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("/api/roles")]
    [ApiController]
    [ApiErrorHandler, ApiModelValidation, PermissionFilter(RolePermissions.AdministrateRoles)]
    public class RoleController : Controller
    {
        private readonly IRoleService roleService;
        private readonly IRoleModelBuilder roleModelBuilder;

        public RoleController(IRoleService roleService, IRoleModelBuilder roleModelBuilder)
        {
            this.roleService = roleService;
            this.roleModelBuilder = roleModelBuilder;
        }

        [HttpGet]
        [Route("getpermissions")]
        public IActionResult GetPermissions()
        {
            var perms = RolePermissions.All;
            return new ApiResult(perms);
        }

        [HttpPost]
        [Route("create")]
        public void CreateRole(RoleCreateForm data)
        {
            roleService.CreateRole(data.Name, data.Permissions);            
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAllRoles()
        {
            var roles = roleService.GetAllRoles();
            var roleModels = roleModelBuilder.Build(roles.ToArray());
            return new ApiResult(roleModels);
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetRole(GuidIdForm data)
        {
            var role = roleService.GetRole(data.Id);
            var roleModel = roleModelBuilder.Build(role);
            return new ApiResult(roleModel);
        }

        [HttpPost]
        [Route("delete")]
        public void DeleteRole(GuidIdForm data)
        {
            roleService.DeleteRole(data.Id);
        }

        [HttpPost]
        [Route("edit")]
        public void UpdateRole(RoleEditForm data)
        {
            roleService.Update(data.Id, data.Name, data.Permissions);
        }
    }
}
