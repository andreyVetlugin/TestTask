using AisBenefits.Infrastructure.Services;
using AisBenefits.Services.Authorize;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace AisBenefits.Services
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IAuthorizeTokenService authorizeTokenService;

        private Lazy<User> currentUser;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor, IReadDbContext<IBenefitsEntity> readDbContext, IAuthorizeTokenService authorizeTokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            this.readDbContext = readDbContext;
            this.authorizeTokenService = authorizeTokenService;

            currentUser = new Lazy<User>(GetCurrentUserInternal);
        }

        public User GetCurrentUser()
        {
            return currentUser.Value;
        }

        private User GetCurrentUserInternal()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Authorization"];
            if (token == null)
                return null;

            var userId = authorizeTokenService.ResolveUserId(token);
            if (userId == Guid.Empty)
                return null;

            var user = readDbContext.Get<User>().ById(userId).FirstOrDefault();
            if (user == null)
                return null;

            var userToken = authorizeTokenService.CreateToken(user);
            if (userToken != token)
                return null;

            return user;
        }
    }
}
