using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AisBenefits.Infrastructure.Services.UsersAndRoles
{
    public interface IRoleService
    {
        void CreateRole(string name, string[] permissions);
        IQueryable<Role> GetAllRoles();
        Role GetRole(Guid id);
        IQueryable<Role> GetUserRoles(Guid userId);
        void DeleteRole(Guid id);
        void Update(Guid id, string name, string[] permissions);
    }
}
