using DataLayer.Entities;
using System;

namespace AisBenefits.Services.Authorize
{
    public interface IAuthorizeTokenService
    {
        Guid ResolveUserId(string token);

        string CreateToken(User user);
    }
}