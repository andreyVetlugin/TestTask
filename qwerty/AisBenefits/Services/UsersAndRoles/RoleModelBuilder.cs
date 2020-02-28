using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Models.UsersAndRoles.OUTPUT;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Services.UsersAndRoles
{
    public interface IRoleModelBuilder
    {
        RoleModel[] Build(Role[] roles);
        RoleModel Build(Role role);
    }

    public class RoleModelBuilder: IRoleModelBuilder
    {
        public RoleModel[] Build(Role[] roles)
        {
            var list = new List<RoleModel>();

            foreach (var role in roles)
            {
                var temp = Build(role);
                list.Add(temp);
            }

            return list.ToArray();
        }

        public RoleModel Build (Role role)
        {
            return new RoleModel
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = RolePermissionsHelper.Resolve(role.Permissions)
            };
        }
    }
}
