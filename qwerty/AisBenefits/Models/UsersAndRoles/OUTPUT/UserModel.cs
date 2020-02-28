using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.UsersAndRoles.OUTPUT
{
    public class UserModel
    {
        public Guid Id { get; set; }
        
        public string Login { get; set; }       
        

        public string Name { get; set; }

        public string SecondName { get; set; }

        public RoleModel[] Roles { get; set; }
    }
}
