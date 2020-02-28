using System;

namespace AisBenefits.Models.UsersAndRoles.INPUT
{
    public class RoleEditForm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string[] Permissions { get; set; }
    }
}
