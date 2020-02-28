using AisBenefits.Services.Security;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;

namespace AisBenefits.Services.Authorize
{
    public class AuthorizeTokenService : IAuthorizeTokenService
    {
        const string SystemKey = "CegthEybrfkmysqCtrhtnysqRk.xCbcntvs";

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHashService hashService;

        public AuthorizeTokenService(IHttpContextAccessor httpContextAccessor, IHashService hashService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.hashService = hashService;
        }

        public Guid ResolveUserId(string token)
        {
            Guid.TryParseExact(token.Substring(0, 32), "N", out Guid userId);
            return userId;
        }

        public string CreateToken(User user)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var remoteIp = httpContext.Connection.RemoteIpAddress.ToString();
            var hash = hashService.GetHash($"{remoteIp}{user.Password}{user.Login}{SystemKey}");

            return user.Id.ToString().Replace("-", "") + hash;
        }
    }
}
