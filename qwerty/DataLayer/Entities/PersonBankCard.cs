using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataLayer.Entities
{
    public class PersonBankCard : IBenefitsEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PersonRootId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? OutDate { get; set; }

        public PersonBankCardType Type { get; set; }

        public string Number { get; set; }
        public DateTime? ValidThru { get; set; }
    }

    public enum PersonBankCardType {
        Card,
        Account
    }

    public static class PersonBankCardQueryableExtensions
    {
        public static IQueryable<PersonBankCard> ActualByPersonRootId(this IQueryable<PersonBankCard> cards, Guid personRootId)
        {
            return cards.Where(p => !p.OutDate.HasValue && p.PersonRootId == personRootId);
        }

        //public static IQueryable<PersonBankCard> ByPersonId(this IQueryable<PersonBankCard> cards, Guid id)
        //{
        //    return cards.Where(p =>  p.Id == id);
        //}

        public static IQueryable<PersonBankCard> ById(this IQueryable<PersonBankCard> cards, Guid id)
        {
            return cards.Where(p => p.Id == id);
        }

        public static IQueryable<PersonBankCard> ByIds(this IQueryable<PersonBankCard> cards, IEnumerable<Guid> ids)
        {
            return cards.Where(p => ids.Contains(p.Id));
        }

        public static IQueryable<PersonBankCard> ActualByPersonInfoRootIds(this IQueryable<PersonBankCard> cards, IEnumerable<Guid> personInfoRootIds)
        {
            return cards.Where(p => !p.OutDate.HasValue && personInfoRootIds.Contains(p.PersonRootId));
        }

        public static IQueryable<PersonBankCard> ByPersonIds(this IQueryable<PersonBankCard> cards, IEnumerable<Guid> ids)
        {
            return cards.Where(p => ids.Contains(p.PersonRootId));
        }
    }
}
