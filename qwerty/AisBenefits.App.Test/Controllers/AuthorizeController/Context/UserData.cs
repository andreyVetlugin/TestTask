using DataLayer.Entities;

namespace AisBenefits.App.Test.Controllers.AuthorizeController.Context
{
    class UserData
    {
        public string UserPassword;
        public readonly User User;
        public readonly Role[] Roles;

        public UserData(string userPassword, User user, Role[] roles)
        {
            UserPassword = userPassword;
            User = user;
            Roles = roles;
        }
    }
}
