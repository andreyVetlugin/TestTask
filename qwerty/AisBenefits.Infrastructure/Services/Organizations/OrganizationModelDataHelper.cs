using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Organizations
{
    using  OrganizationModelDataResult = ModelDataResult<OrganizationModelData>;
    using AllOrganizationsModelDataResult = ModelDataResult<List<OrganizationModelData>>;

    public static class OrganizationModelDataHelper
    {
        public static OrganizationModelDataResult GetOrganizationModelDataResult(this IReadDbContext<IBenefitsEntity> readDbContext, Guid organizationId)
        {
            var organization = readDbContext.Get<Organization>()
                .ById(organizationId)
                .FirstOrDefault();

            if (organization == null)
                return OrganizationModelDataResult.BuildFormError("Указанная организация не существует");

            var hasUsages = readDbContext.Get<WorkInfo>()
                .ByOrganizationId(organizationId)
                .Any();

            return OrganizationModelDataResult.BuildSucces(
                new OrganizationModelData(organization, hasUsages)
                );
        }

        public static AllOrganizationsModelDataResult GetAllOrganizationsModelDataResult(this IReadDbContext<IBenefitsEntity> readDbContext)
        {
            var organizations = readDbContext.Get<Organization>().ToList();

            var usages = readDbContext.Get<WorkInfo>()
                .GroupBy(w => w.OrganizationId)
                .Select(g => g.Key)
                .ToHashSet();

            return AllOrganizationsModelDataResult.BuildSucces(
                organizations.Select(o => new OrganizationModelData(o, usages.Contains(o.Id))).ToList()
                );
        }
    }

    public class OrganizationModelData
    {
        public  Organization Organization { get; }
        public  bool HasUsages { get; }

        public OrganizationModelData(Organization organization, bool hasUsages)
        {
            Organization = organization;
            HasUsages = hasUsages;
        }
    } 
}
