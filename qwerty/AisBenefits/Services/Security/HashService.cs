using System.Security.Cryptography;
using System.Text;

namespace AisBenefits.Services.Security
{
    public class HashService : IHashService
    {
        public string GetHash(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.Default.GetBytes(data));

                StringBuilder sBuilder = new StringBuilder(64);

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sBuilder.Append(hashBytes[i].ToString("x2"));
                }

                var hash = sBuilder.ToString();
                return hash;
            }
        }
    }
}
