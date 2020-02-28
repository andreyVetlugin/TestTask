namespace AisBenefits.Services.Security
{
    public interface IHashService
    {
        string GetHash(string data);
    }
}