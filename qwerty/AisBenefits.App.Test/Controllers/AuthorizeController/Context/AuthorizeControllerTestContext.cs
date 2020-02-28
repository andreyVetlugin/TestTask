using System;
using System.Linq;
using DataLayer.Entities;

namespace AisBenefits.App.Test.Controllers.AuthorizeController.Context
{
    class AuthorizeControllerTestContext : IDisposable
    {
        public readonly UserData UserData = new UserData(
            "1",
            new User { Login = "1", Password = Infrastructure.Services.Security.PasswordHasher.GetHash("1") },
            new Role[0]
            );

        public readonly ControllerTestContext ControllerContext;
        public readonly AisBenefits.Controllers.AutorizeController Controller;

        public AuthorizeControllerTestContext()
        {
            ControllerContext = new ControllerTestContext(
                sp => sp.GetHashCode(),
                new IBenefitsEntity[]
                    {
                        UserData.User
                    }
                    .Concat(UserData.Roles)
                    .ToList()
                );

            Controller = ControllerContext.GetController<AisBenefits.Controllers.AutorizeController>();
        }

        public void Dispose()
        {
            ControllerContext.Dispose();
        }
    }
}
