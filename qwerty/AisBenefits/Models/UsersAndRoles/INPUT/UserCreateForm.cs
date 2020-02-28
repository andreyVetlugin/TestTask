using System;
using System.ComponentModel.DataAnnotations;

namespace AisBenefits.Models.UsersAndRoles.INPUT
{
    public class UserCreateForm
    {
        [Required]
        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string SecondName { get; set; }

        public Guid[] RoleIds { get; set; }
    }
}
