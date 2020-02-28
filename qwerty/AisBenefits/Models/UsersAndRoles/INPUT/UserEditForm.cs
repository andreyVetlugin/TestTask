using System;

namespace AisBenefits.Models.UsersAndRoles.INPUT
{
    public class UserEditForm
    {
        public Guid Id { get; set; }
       
        public string Login { get; set; }
        
        public string Password { get; set; }

        public string Name { get; set; }

        public string SecondName { get; set; }

        public Guid[] RoleIds { get; set; }
    }
}
