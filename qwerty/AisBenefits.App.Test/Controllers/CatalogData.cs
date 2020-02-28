using AisBenefits.Infrastructure.Helpers;
using DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace AisBenefits.App.Test.Controllers
{
    static class CatalogData
    {
        public static Guid OneXOrganization = Guid.NewGuid();
        public static Guid OneDotOneFiveXOrganization = Guid.NewGuid();

        public static ICollection<IBenefitsEntity> AddFilledCatalog(this ICollection<IBenefitsEntity> collection)
        {
            collection.Add(new DsPerc
            {
                Id = Guid.NewGuid(),
                AgeDays = WorkAge.GetAgeDays(5, 0, 0),
                Amount = 50,
                GenderType = DsPercGenderType.General,
                Period = new DateTime(2000, 1, 1)
            });

            collection.Add(new MinExtraPay
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(2000, 1, 1),
                Value = 2600
            });

            collection.Add(new Organization
            {
                Id = OneXOrganization,
                OrganizationName = "Организация",
                Multiplier = 1
            });

            collection.Add(new Organization
            {
                Id = OneDotOneFiveXOrganization,
                OrganizationName = "Организация",
                Multiplier = 1
            });

            return collection;
        }
    }
}
