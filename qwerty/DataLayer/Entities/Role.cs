using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataLayer.Entities
{
    public class Role: IBenefitsEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Permissions { get; set; }
    }

    public static class RoleQuereableExtensions
    {
        public static IQueryable<Role> ById(this IQueryable<Role> roles, Guid Id) =>
            roles.Where(r => r.Id == Id);
    }
}
