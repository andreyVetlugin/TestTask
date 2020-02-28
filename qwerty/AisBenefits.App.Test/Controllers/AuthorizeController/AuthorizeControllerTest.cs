using AisBenefits.App.Test.Controllers.AuthorizeController.Context;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AisBenefits.App.Test.Controllers.AuthorizeController
{
    public class AuthorizeControllerTest
    {
        [Test]
        public async Task AuthorizeShouldSuccess()
        {
            using (var context = new AuthorizeControllerTestContext())
            {
                var result = context.Controller.Autorize(new Models.Authorize.AuthForm
                {
                    Login = context.UserData.User.Login,
                    Password = context.UserData.UserPassword
                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                Assert.AreEqual(200, context.ControllerContext.HttpContext.Response.StatusCode);
            }
        }

        [Test]
        public async Task AuthorizeShouldFail()
        {
            using (var context = new AuthorizeControllerTestContext())
            {
                var result = context.Controller.Autorize(new Models.Authorize.AuthForm
                {
                    Login = context.UserData.User.Login,
                    Password = "Корявый пароль"
                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                Assert.AreEqual(403, context.ControllerContext.HttpContext.Response.StatusCode);
            }
        }
    }
}
