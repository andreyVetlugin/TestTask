using DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace AisBenefits.Infrastructure.Services.Organizations
{
    public interface IOrganizationService
    {
        void Create(string name, decimal multiplier);
        void Edit(Guid id, string name, decimal multiplier);
        List<Organization> GetAll();

    }
}
