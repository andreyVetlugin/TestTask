using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AisBenefits.Infrastructure.Services.UsersAndRoles
{
    public interface IUserService
    {
        void CreateUser(string login, string password, string name, string secondName, Guid[] roleIds);
        IQueryable<User> GetAllUsers();
        User GetUser(Guid id);
        void DeleteUser(Guid id);
        void Update(Guid id, string login, string password, string name, string secondName, Guid[] roleIds);
    }
}
