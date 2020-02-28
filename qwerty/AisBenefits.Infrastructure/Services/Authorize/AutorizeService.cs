using AisBenefits.Infrastructure.Services.UsersAndRoles;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System.Linq;

namespace AisBenefits.Infrastructure.Services.Authorize
{
    public class AutorizeService : IAutorizeService
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IPasswordHasher passwordHasher;

        public AutorizeService(IReadDbContext<IBenefitsEntity> readDbContext, IPasswordHasher passwordHasher)
        {
            this.readDbContext = readDbContext;
            this.passwordHasher = passwordHasher;
        }

        public IAutorizeServiceResult Authorize(string login, string password)
        {
            var passwordHash = passwordHasher.GetHash(password);

            var user = readDbContext.Get<User>()
                .ByLoginAndPassword(login, passwordHash)
                .FirstOrDefault();

            if (user != null)
                return BuildSuccess(user);
            else
                return new AutorizeServiceErrorResult("Неверный логин и/или пароль");
        }

        private AutorizeServiceSuccessResult BuildSuccess(User user)
        {
            var roles = readDbContext.Get<RoleUserLink>().ByUserId(user.Id).Join(readDbContext.Get<Role>(), p => p.RoleId, c => c.Id, (p, c) => c);

            return new AutorizeServiceSuccessResult(user, roles.ToList());
        }
    }
}
