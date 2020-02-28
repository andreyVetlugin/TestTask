using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DataLayer.Entities
{
    public class RoleUserLink: IBenefitsEntity
    {
        [Key]
        public Guid RoleId { get; set; }
        [Key]
        public Guid UserId { get; set; }
    }


    public static class RoleLinkUserQuereableExtensions
    {
        public static IQueryable<RoleUserLink> ByUserId(this IQueryable<RoleUserLink> roleLinkUser, Guid userId) =>
            roleLinkUser.Where(r => r.UserId == userId);

        public static IQueryable<RoleUserLink> ByRoleId(this IQueryable<RoleUserLink> roleLinkUser, Guid roleId) =>
            roleLinkUser.Where(r => r.RoleId == roleId);
    }

}
