using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Entities
{
    public class Organization: IBenefitsEntity
    {
        public Guid Id { get; set; }
       
        public string OrganizationName { get; set; }

        public decimal Multiplier { get; set; }
        
    }

    public static class OrganizationQueryableExtensions
    {
        public static IQueryable<Organization> ById(this IQueryable<Organization> workInfos, Guid id) =>
            workInfos.Where(r => r.Id == id);
        public static IQueryable<Organization> ByIds(this IQueryable<Organization> workInfos, IEnumerable<Guid> ids) =>
            workInfos.Where(r => ids.Contains(r.Id));

        public static IQueryable<Organization> ByName(this IQueryable<Organization> query, string name) =>
            query.Where(o => o.OrganizationName.ToLower() == name.ToLower());
    }

}


