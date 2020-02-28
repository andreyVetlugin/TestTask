using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.Services.Security
{
    public static class PasswordHasher
    {
        public static string GetHash(string password)
        {
            return HashService.GetHash(password);
        }
    }
}
