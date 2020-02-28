using DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace AisBenefits.Infrastructure.Services.Organizations
{
    public interface IFunctionService
    {
        void Create(string name);
        void Edit(Guid id, string name);
        List<Function> GetAll();
    }
}
