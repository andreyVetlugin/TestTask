using AisBenefits.Infrastructure.Permissions;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AisBenefits.Attributes.PermAuth2
{
    public interface IUsersPermissionGetService
    {
        HashSet<string> GetPermissions(Guid userId);
    }

    public class UsersPermissionGetService: IUsersPermissionGetService
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public UsersPermissionGetService(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public HashSet<string> GetPermissions(Guid userId)
        {
            return (from link in readDbContext.Get<RoleUserLink>().ByUserId(userId)
                    join role in readDbContext.Get<Role>() on link.RoleId equals role.Id
                    select role.Permissions)
                    .AsEnumerable()
                    .SelectMany(ps => RolePermissionsHelper.Resolve(ps))
                    .ToHashSet();
        }
    }
}
