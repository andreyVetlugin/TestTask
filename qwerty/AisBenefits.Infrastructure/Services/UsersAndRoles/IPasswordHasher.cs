namespace AisBenefits.Infrastructure.Services.UsersAndRoles
{
    public interface IPasswordHasher
    {
        string GetHash(string password);
    }
}
