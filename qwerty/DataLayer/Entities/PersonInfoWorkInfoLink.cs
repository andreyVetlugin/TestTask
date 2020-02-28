using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DataLayer.Entities
{
    public class PersonInfoWorkInfoLink: IBenefitsEntity
    {
        [Key]
        public Guid PersonInfoId { get; set; }
        [Key]
        public Guid WorkInfoId { get; set; }
    }


    public static class PersonWorkInfoLinkQuereableExtensions
    {
        public static IQueryable<PersonInfoWorkInfoLink> ByPersonInfoId(this IQueryable<PersonInfoWorkInfoLink> personWorkInfoLink, Guid personInfoId) =>
            personWorkInfoLink.Where(r => r.PersonInfoId == personInfoId);

        public static IQueryable<PersonInfoWorkInfoLink> ByWorkInfoId(this IQueryable<PersonInfoWorkInfoLink> personWorkInfoLink, Guid workInfoId) =>
            personWorkInfoLink.Where(r => r.WorkInfoId == workInfoId);
    }

}
