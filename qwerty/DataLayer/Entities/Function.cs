using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Entities
{
    public class Function: IBenefitsEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public static class FunctionQueryableExtensions
    {
        public static IQueryable<Function> ByName(this IQueryable<Function> query, string name) =>
            query.Where(f => f.Name.ToLower() == name.ToLower());
        public static IQueryable<Function> ById(this IQueryable<Function> functions, Guid id) =>
            functions.Where(r => r.Id == id);

        public static IQueryable<Function> ByIds(this IQueryable<Function> functions, IEnumerable<Guid> ids) =>
            functions.Where(r => ids.Contains(r.Id));
    }
}
