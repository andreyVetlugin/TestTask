using DataLayer.Entities;
using System.Collections.Generic;

namespace AisBenefits.Infrastructure.Services.Authorize
{
    public interface IAutorizeService
    {
        IAutorizeServiceResult Authorize(string login, string password);
    }

    public interface IAutorizeServiceResult
    {
    }

    public class AutorizeServiceSuccessResult : IAutorizeServiceResult
    {
        public User User { get; private set; }
        public IReadOnlyList<Role> Roles { get; private set; }

        public AutorizeServiceSuccessResult(User user, IReadOnlyList<Role> roles)
        {
            User = user;
            Roles = roles;
        }
    }

    public class AutorizeServiceErrorResult : IAutorizeServiceResult
    {
        public string Error { get; private set; }

        public AutorizeServiceErrorResult(string error)
        {
            Error = error;
        }
    }
}