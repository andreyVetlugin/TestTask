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
    [Route("/api/users"), ]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministrateUsers)]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IUserModelBuilder userModelBuilder;

        public UserController(IUserService userService, IUserModelBuilder userModelBuilder)
        {
            this.userService = userService;
            this.userModelBuilder = userModelBuilder;
        }

        [HttpPost]
        [Route ("create")]
        public void Create(UserCreateForm data)
        {
            userService.CreateUser(data.Login,data.Password, data.Name, data.SecondName, data.RoleIds);
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAllUsers()
        {
            var users = userService.GetAllUsers();

            var userModels = userModelBuilder.Build(users.ToArray());
            return new ApiResult(userModels);
        }
        
        [HttpPost]
        [Route("get")]
        public IActionResult GetUser(GuidIdForm data)
        {           
            var user = userService.GetUser(data.Id);

            var userModel = userModelBuilder.Build(user);
            return new ApiResult(userModel);
        }
        
        [HttpPost]
        [Route("delete")]
        public void DeleteUser(GuidIdForm data)
        {
            userService.DeleteUser(data.Id);
        }
        
        [HttpPost]
        [Route("edit")]
        public void UpdateUser(UserEditForm data)
        {
            userService.Update(data.Id ,data.Login, data.Password, data.Name, data.SecondName, data.RoleIds);
        }
    }
}