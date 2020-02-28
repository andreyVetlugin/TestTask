using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Organizations
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly ILogBuilder logBuilder;

        public OrganizationService(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext, ILogBuilder logBuilder)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
            this.logBuilder = logBuilder;
        }

        public void Create(string name, decimal multiplier)
        {
            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                Multiplier = multiplier,
                OrganizationName = name
            };

            writeDbContext.Add(organization);
            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.OrganizationCreate, organization.Id);
            writeDbContext.Add(infoLog);
            writeDbContext.SaveChanges();
        }

        public void Edit(Guid id, string name, decimal multiplier)
        {
            var organization = readDbContext.Get<Organization>()
                .ById(id)
                .FirstOrDefault();

            writeDbContext.Attach(organization);
            organization.OrganizationName = name;
            organization.Multiplier = multiplier;
            
            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.OrganizationEdit, organization.Id);
            writeDbContext.Add(infoLog);
            writeDbContext.SaveChanges();
        }

        public List<Organization> GetAll()
        {
            return readDbContext.Get<Organization>().ToList();
        }
    }
}
