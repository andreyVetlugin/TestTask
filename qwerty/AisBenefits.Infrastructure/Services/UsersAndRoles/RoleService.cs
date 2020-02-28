using AisBenefits.Infrastructure.Permissions;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AisBenefits.Infrastructure.Services.UsersAndRoles
{

    public class RoleService : IRoleService
    {

        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public RoleService(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
        }

        public void CreateRole(string name, string[] permissions)
        {
            var role = new Role {
                Id = Guid.NewGuid(),
                Name = name,
                Permissions = RolePermissionsHelper.Create(permissions)
            };

            writeDbContext.Add(role);
            writeDbContext.SaveChanges();
        }

        public IQueryable<Role> GetAllRoles()
        {
            return readDbContext.Get<Role>();
        }

        public Role GetRole(Guid id)
        {
            return readDbContext.Get<Role>().FirstOrDefault(c=>c.Id==id);
        }


        public IQueryable<Role> GetUserRoles(Guid userId)
        {
            var roles = readDbContext.Get<RoleUserLink>().ByUserId(userId).Join(readDbContext.Get<Role>(), p => p.RoleId, c => c.Id, (p, c) => c);
            return roles;
        }


        public void DeleteRole(Guid id)
        {
            var role = GetRole(id);
            writeDbContext.Remove(role);
            writeDbContext.SaveChanges();
        }

        public void Update(Guid id, string name, string[] permissions)
        {
            var role = readDbContext.Get<Role>().FirstOrDefault(c => c.Id == id);
            writeDbContext.Attach(role);
            role.Name = name;
            role.Permissions = RolePermissionsHelper.Create(permissions);
            writeDbContext.SaveChanges();
        }
    }
}
