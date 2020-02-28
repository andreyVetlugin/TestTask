using System;
using System.Linq;
using AisBenefits.Infrastructure.Services.Security;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.UsersAndRoles
{
    public class UserService : IUserService
    {

        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        public UserService(IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
        }

        public void CreateUser(string login, string password, string name, string secondName, Guid[] roleIds)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                Name = name,
                SecondName = secondName,
                Password = PasswordHasher.GetHash(password)
            };

            writeDbContext.Add(user);


            foreach (var roleId in roleIds)
            {
                var roleLinkUser = new RoleUserLink
                {
                    RoleId = roleId,
                    UserId = user.Id
                };
                writeDbContext.Add(roleLinkUser);
            }
            writeDbContext.SaveChanges();
        }

        public void DeleteUser(Guid id)
        {
            var user = GetUser(id);
            writeDbContext.Remove<User>(user);
            writeDbContext.SaveChanges();
        }

        public IQueryable<User> GetAllUsers()
        {
           return readDbContext.Get<User>();
        }

        public User GetUser(Guid id)
        {
            return readDbContext.Get<User>().FirstOrDefault(c=>c.Id==id);
        }

        public void Update(Guid id, string login, string password, string name, string secondName, Guid[] roleIds)
        {
            var user = readDbContext.Get<User>().FirstOrDefault(c => c.Id == id);

            if (user == null)
                throw new InvalidOperationException();

            var links = readDbContext.Get<RoleUserLink>().Where(c => c.UserId == id);
            writeDbContext.RemoveRange(links.ToArray());
            writeDbContext.Attach(user);
            user.Login = login;
            if(!string.IsNullOrEmpty(password))
                user.Password = PasswordHasher.GetHash(password);
            user.Name = name;
            user.SecondName = secondName;

            foreach (var roleId in roleIds)
            {
                var roleLinkUser = new RoleUserLink
                {
                    RoleId = roleId,
                    UserId = user.Id
                };
                writeDbContext.Add(roleLinkUser);
            }
            writeDbContext.SaveChanges();
        }
    }
}
