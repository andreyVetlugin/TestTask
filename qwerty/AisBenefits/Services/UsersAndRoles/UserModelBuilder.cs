using AisBenefits.Infrastructure.Services.UsersAndRoles;
using AisBenefits.Models.UsersAndRoles.OUTPUT;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Services.UsersAndRoles
{

    public interface IUserModelBuilder
    {
        UserModel Build(User user);
        UserModel[] Build(User[] user);
    }

    public class UserModelBuilder : IUserModelBuilder
    {
        private readonly IRoleService roleService;
        private readonly IRoleModelBuilder roleModelBuilder;

        public UserModelBuilder(IRoleService roleService, IRoleModelBuilder roleModelBuilder)
        {
            this.roleService = roleService;
            this.roleModelBuilder = roleModelBuilder;
        }

        public UserModel Build(User user)
        {
            var roles = roleService.GetUserRoles(user.Id);
            var roleModels = roleModelBuilder.Build(roles.ToArray());

            return new UserModel
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                SecondName = user.SecondName,
                Roles = roleModels
            };
        }


        public UserModel[] Build (User[] users)
        {
            var userModelList = new List<UserModel>();

            foreach (var user in users)
            {
                var temp = Build(user);
                userModelList.Add(temp);
            }

            return userModelList.ToArray();
        }


    }
}
