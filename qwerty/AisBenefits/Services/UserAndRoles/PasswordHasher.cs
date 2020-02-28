using AisBenefits.Infrastructure.Services.UsersAndRoles;
using AisBenefits.Services.Security;

namespace AisBenefits.Services.UserAndRoles
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly IHashService hashService;

        public PasswordHasher(IHashService hashService)
        {
            this.hashService = hashService;
        }

        public string GetHash(string password)
        {
            return hashService.GetHash(password);
        }
    }
}
