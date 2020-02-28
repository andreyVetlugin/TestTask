using System;
using DataLayer.Entities;
using OldDataImport.App.Entities;

namespace OldDataImport.App
{
    public static class OrganizationDbfMapper
    {
        public static Organization Map(IOrgan organ)
        {
            return new Organization
            {
                Id = Guid.NewGuid(),
                Multiplier = organ.KOEFF,
                OrganizationName = organ.ORGAN
            };
        }

        public static Organization Map(IStag stag)
        {
            return new Organization
            {
                Id = Guid.NewGuid(),
                Multiplier = stag.KOEF,
                OrganizationName = stag.UCHREGD
            };
        }
    }
}
