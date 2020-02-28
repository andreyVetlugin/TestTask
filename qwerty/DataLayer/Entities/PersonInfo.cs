using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Entities
{
    public class PersonInfo : IBenefitsEntity
    {
        public Guid Id { get; set; }
        public Guid? NextId { get; set; }
        public Guid RootId { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? OutdateTime { get; set; }

        public int Number { get; set; }

        public Guid EmployeeTypeId { get; set; }

        public Guid DistrictId { get; set; }

        public Guid PensionTypeId { get; set; }
        public Guid PayoutTypeId { get; set; }
        public DateTime? PensionEndDate { get; set; }
        public Guid AdditionalPensionId { get; set; }


        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string Birthplace { get; set; }
        public DateTime BirthDate { get; set; }
        public char Sex { get; set; }

        public string SNILS { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }

        public string DocTypeId { get; set; }
        public string CodeEgisso { get; set; }
        public string DocNumber { get; set; }
        public string DocSeria { get; set; }
        public string Issuer { get; set; }
        public DateTime? IssueDate { get; set; }

        public string PensionCaseNumber { get; set; }


        public bool Approved { get; set; }
        public DateTime? DocsSubmitDate { get; set; }
        public DateTime? DocsDestinationDate { get; set; }

        public bool StoppedSolutions { get; set; }

        public PersonInfo CreateNext(Guid id, DateTime date)
        {
            var person = MemberwiseClone() as PersonInfo;
            person.Id = id;
            person.CreateTime = date;

            NextId = id;
            OutdateTime = date;

            return person;
        }
    }


    public static class PersonInfoQueryableExtensions
    {
        public static IQueryable<PersonInfo> ByFio(this IQueryable<PersonInfo> query, string surName, string name, string middleName)
        {
            bool noSurName;
            bool noName;

            if (!(noSurName = string.IsNullOrWhiteSpace(surName)))
            {
                surName = surName.ToLower().Trim();
                query = query.Where(p => p.SurName.ToLower().Trim() == surName);
            }
            if (!(noName = string.IsNullOrWhiteSpace(name)))
            {
                name = name.ToLower().Trim();
                query = query.Where(p => p.Name.ToLower().Trim() == name);
            }
            if (!string.IsNullOrWhiteSpace(middleName))
            {
                middleName = middleName.ToLower().Trim();
                query = query.Where(p => p.MiddleName.ToLower().Trim() == middleName);
            }
            return noSurName || noName ? query.Where(p => false) : query;
        }

        public static IQueryable<PersonInfo> BySurNames(this IQueryable<PersonInfo> query, IEnumerable<string> names)
        {
            names = names.Select(n => n.Trim().ToLower());
            return query.Where(p => names.Contains(p.SurName.Trim().ToLower()));
        }
        public static IQueryable<PersonInfo> ByDocNumbers(this IQueryable<PersonInfo> query, IEnumerable<string> numbers) =>
            query.Where(p => numbers.Contains(p.DocNumber));

        public static IQueryable<PersonInfo> OptionalByBirthDate(this IQueryable<PersonInfo> query, DateTime? birthDate) =>
            birthDate.HasValue ? query.Where(p => p.BirthDate == birthDate) : query;

        public static IQueryable<PersonInfo> Root(this IQueryable<PersonInfo> infos) =>
            infos.Where(p => p.Id == p.RootId);
        public static IQueryable<PersonInfo> ByNumber(this IQueryable<PersonInfo> infos, int number) =>
            infos.Where(r => !r.NextId.HasValue && r.Number == number);
        public static IQueryable<PersonInfo> ByNumbers(this IQueryable<PersonInfo> infos, IEnumerable<int> numbers) =>
            infos.Where(r => !r.NextId.HasValue && numbers.Contains(r.Number));

        public static IQueryable<PersonInfo> ByRootId(this IQueryable<PersonInfo> infos, Guid rootId) =>
            infos.Where(r => r.RootId == rootId && r.NextId == null);

        public static IQueryable<PersonInfo> ById(this IQueryable<PersonInfo> infos, Guid id) =>
            infos.Where(r => r.Id == id);

        public static IQueryable<PersonInfo> ByIds(this IQueryable<PersonInfo> infos, IEnumerable<Guid> ids) =>
            infos.Where(r => ids.Contains(r.Id));

        public static IQueryable<PersonInfo> ByRootIds(this IQueryable<PersonInfo> infos, IEnumerable<Guid> rootIds) =>
         infos.Where(r => rootIds.Contains(r.RootId) && r.NextId == null);

        public static IQueryable<PersonInfo> HistoryByRootIds(this IQueryable<PersonInfo> infos, IEnumerable<Guid> rootIds) =>
            infos.Where(r => rootIds.Contains(r.RootId));

        public static IQueryable<PersonInfo> ByRootIdsAndConfirmedExp(this IQueryable<PersonInfo> infos, IEnumerable<Guid> rootIds) =>
            infos.Where(r => rootIds.Contains(r.RootId) && r.NextId == null && r.Approved);

        public static IQueryable<PersonInfo> AllArchive(this IQueryable<PersonInfo> infos) =>
            infos.Where(r => r.StoppedSolutions);

        public static IQueryable<PersonInfo> ArchiveByRootIds(this IQueryable<PersonInfo> infos, IEnumerable<Guid> rootIds) =>
            infos.Where(r => rootIds.Contains(r.RootId) && r.StoppedSolutions);

        public static IQueryable<PersonInfo> ByCurrentIds(this IQueryable<PersonInfo> infos, IEnumerable<Guid> ids) =>
           infos.Where(r => ids.Contains(r.Id));

        public static IQueryable<PersonInfo> Active(this IQueryable<PersonInfo> infos) =>
            infos.Where(r => r.NextId == null && !r.StoppedSolutions);

        public static IQueryable<int> OnlyNumbers(this IQueryable<PersonInfo> infos) =>
            infos.Select(c => c.Number);

        public static IQueryable<PersonInfo> ActiveApproved(this IQueryable<PersonInfo> infos) =>
            infos.Where(r => r.NextId == null && r.Approved && !r.StoppedSolutions);

        public static IQueryable<PersonInfo> ApprovedByPayoutTypeIds(this IQueryable<PersonInfo> infos, IEnumerable<Guid> payoutTypeIds) =>
            infos.Where(r => payoutTypeIds.Contains(r.PayoutTypeId) && r.NextId == null && r.Approved);

        public static IQueryable<PersonInfo> Search(this IQueryable<PersonInfo> infos, string search, bool archive) =>
            infos.Where(r => r.NextId == null && r.StoppedSolutions == archive && (search == null || string.Concat(r.Number, r.SurName, r.Name, r.MiddleName ?? string.Empty).Replace(" ", "").ToLower().Contains(search.Replace(" ", "").ToLower())));

        public static IQueryable<PersonInfo> Actual(this IQueryable<PersonInfo> query) =>
            query.Where(p => p.NextId == null);
    }

    public static class PersonInfoExtensions
    {
        public static bool IsRoot(this PersonInfo personInfo) =>
            personInfo.RootId == personInfo.Id;
    }
}
