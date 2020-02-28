using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.UsersAndRoles.OUTPUT
{
    public class RoleModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string[] Permissions { get; set; }
    }
}
