using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AisBenefits.Services.Zags
{
    public static class ZagsStopActService
    {
        public static void CreateZagsStopActData(this IWriteDbContext<IBenefitsEntity> write, IEnumerable<ZagsStopActData> data)
        {
            foreach(var d in data)
            {
                if (d.New)
                {
                    write.Add(d.Act);
                }
            }
        }

        public static IEnumerable<ZagsStopActData> GetZagsStopActData(this IReadDbContext<IBenefitsEntity> read, IFormFile excel)
        {
            var data = excel.GetExcelDatas().GroupBy(d => d.Number).Select(d => d.First()).ToList();

            var fiosd = data.Where(a => a.HasFio).ToDictionary(a => (a.SurName, a.FirstName, a.MiddleName, a.BirthDate), a => a, new FiodComparer());
            var docs = data.Where(a => a.HasDocument).ToDictionary(a => (a.DocSeria, a.DocNumber), a => a);

            var persons =
                read.Get<PersonInfo>()
                .Active()
                .BySurNames(fiosd.Keys.Select(a => a.Item1))
                .AsEnumerable()
                .Where(a => fiosd.ContainsKey((a.SurName, a.Name, a.MiddleName, a.BirthDate)))
                .Concat(
                    read.Get<PersonInfo>()
                    .Active()
                    .ByDocNumbers(docs.Keys.Select(a => a.DocNumber))
                    .AsEnumerable()
                    .Where(p => docs.ContainsKey((p.DocSeria, p.DocNumber)))
                    )
                .GroupBy(p => p.RootId)
                .Select(p => p.First())
                .ToList();

            var acts = read.Get<ZagsStopAct>()
                .ByPersonInfoRootIds(persons.Select(p => p.RootId))
                .AsEnumerable()
                .ToDictionary(a => a.Number);

            foreach (var person in persons)
            {
                var hasAct = docs.TryGetValue((person.DocSeria, person.DocNumber), out var d) ||
                    fiosd.TryGetValue((person.SurName, person.Name, person.MiddleName, person.BirthDate), out d);
                    

                if (!hasAct)
                    continue;

                bool @new;
                if ((@new = !acts.TryGetValue(d.Number, out var act)))
                    act = new ZagsStopAct
                    {
                        Id = Guid.NewGuid(),
                        Date = d.Date,
                        Number = d.Number,
                        PersonInfoRooId = person.RootId,
                        State = ZagsStopActState.New
                    };

                yield return new ZagsStopActData(person, act, @new);
            }
        }

        public static IEnumerable<ZagsStopActData> GetZagsStopActData(this IReadDbContext<IBenefitsEntity> read)
        {
            return (from act in read.Get<ZagsStopAct>().New()
                    join person in read.Get<PersonInfo>().Actual() on act.PersonInfoRooId equals person.RootId
                    select new { act, person })
                    .AsEnumerable()
                    .Select(p => new ZagsStopActData(p.person, p.act, false));
        }

        public static ZagsStopActData GetZagsStopActData(this IReadDbContext<IBenefitsEntity> read, Guid actId)
        {
            return (from act in read.Get<ZagsStopAct>().ById(actId)
                    join person in read.Get<PersonInfo>().Actual() on act.PersonInfoRooId equals person.RootId
                    select new { act, person })
                    .AsEnumerable()
                    .Select(p => new ZagsStopActData(p.person, p.act, false))
                    .FirstOrDefault();
        }

        static IEnumerable<ExcelData> GetExcelDatas(this IFormFile excel)
        {
            var doc = WorkbookFactory.Create(excel.OpenReadStream());

            var sheet = doc.GetSheetAt(0);

            var rowEnumerator = sheet.GetRowEnumerator();
            rowEnumerator.MoveNext();
            while (rowEnumerator.MoveNext())
            {
                var row = rowEnumerator.Current as IRow;

                var number = row.GetCell(1).StringCellValue;

                var date = row.GetCell(2).DateCellValue;
                  
                var surName = row.GetCell(9).StringCellValue;
                var firstName = row.GetCell(10).StringCellValue;
                var middleName = row.GetCell(11).StringCellValue;

                if (!DateTime.TryParse(row.GetCell(13).StringCellValue, out var birthDate))
                    continue;

                string docSeria = null;
                string docNumber = null;

                var docSeriaDocMatch = Regex.Match(row.GetCell(61).StringCellValue, @"^(\d{2} \d{2}) (\d{6})$");
                if (docSeriaDocMatch.Success)
                {
                    docSeria = docSeriaDocMatch.Groups[1].Value.Replace(" ", "");
                    docNumber = docSeriaDocMatch.Groups[2].Value;
                }

                yield return new ExcelData(number, date, surName, firstName, middleName, birthDate, docSeria, docNumber);
            }
        }

        class ExcelData
        {
            public string Number { get; private set; }
            public DateTime Date { get; private set; }

            public string SurName { get; private set; }
            public string FirstName { get; private set; }
            public string MiddleName { get; private set; }
            public DateTime BirthDate { get; private set; }

            public string DocSeria { get; private set; }
            public string DocNumber { get; private set; }

            public bool HasFio => !string.IsNullOrEmpty(SurName);
            public bool HasDocument => !string.IsNullOrEmpty(DocNumber);

            public ExcelData(string number, DateTime date, string surName, string firstName, string middleName, DateTime birthDate, string docSeria, string docNumber)
            {
                Number = number;
                Date = date;
                SurName = surName;
                FirstName = firstName;
                MiddleName = middleName;
                BirthDate = birthDate;
                DocSeria = docSeria;
                DocNumber = docNumber;
            }
        }

        class FiodComparer : IEqualityComparer<(string sur, string first, string middle, DateTime birthDate)>
        {
            public bool Equals((string sur, string first, string middle, DateTime birthDate) x, (string sur, string first, string middle, DateTime birthDate) y)
            {
                return
                    x.sur.Equals(y.sur, StringComparison.OrdinalIgnoreCase) &&
                    x.first.Equals(y.first, StringComparison.OrdinalIgnoreCase) &&
                    x.middle.Equals(y.middle, StringComparison.OrdinalIgnoreCase) &&
                    x.birthDate == y.birthDate;
            }

            public int GetHashCode((string sur, string first, string middle, DateTime birthDate) obj)
            {
                return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.sur) +
                       StringComparer.OrdinalIgnoreCase.GetHashCode(obj.first) +
                       StringComparer.OrdinalIgnoreCase.GetHashCode(obj.middle) +
                       obj.birthDate.GetHashCode();
            }
        }
    }

    public class ZagsStopActData
    {
        public PersonInfo Person { get; private set; }
        public ZagsStopAct Act { get; private set; }
        public bool New { get; private set; }

        public ZagsStopActData(PersonInfo person, ZagsStopAct act, bool @new)
        {
            Person = person;
            Act = act;
            New = @new;
        }
    }
}
