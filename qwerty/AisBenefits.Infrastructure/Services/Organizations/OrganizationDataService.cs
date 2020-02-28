using System;
using System.Linq;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Organizations
{
    public static class OrganizationDataService
    {
        public static bool EnsureOrganizationExist(this IReadDbContext<IBenefitsEntity> read,
            IWriteDbContext<IBenefitsEntity> write, string name, out Organization organization)
        {
            var organizationName = name.Trim();

            organization = read.Get<Organization>()
                .ByName(organizationName)
                .FirstOrDefault();

            var newOrganization = organization == null &&
                                  (organization = new Organization
                                  {
                                      Id = Guid.NewGuid(),
                                      Multiplier = 1,
                                      OrganizationName = organizationName
                                  }) != null;
            if (newOrganization)
                write.Add(organization);

            return newOrganization;
        }
    }
}
