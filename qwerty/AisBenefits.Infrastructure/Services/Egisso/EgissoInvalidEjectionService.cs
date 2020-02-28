using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AisBenefits.Infrastructure.Services.Egisso
{
    public static class EgissoInvalidEjectionService
    {
        public static void WriteInvalidEgissoFactCsv<TContext>(this TContext context, Stream outStream, IEnumerable<IEgissoFactValidationError> facts)
            where TContext: IEgissoEjectionContext
        {
            facts = facts
                .OrderBy(f => (f.Reciever.Person.LastName, f.Reciever.Person.FirstName, f.Reciever.Person.SurName));

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Id;Номер;ФИО;Дата рождения;Ошибка");

            foreach (var r in facts)
            {
                var person = r.Reciever.Person;
                var error = string.Join(".  ", r.Errors);
                var line = string.Join(";", r.RecieverId, r.RecieverNumber, $"{person.LastName} {person.FirstName} {person.SurName}", person.BirthDate.ToShortDateString(), error);
                sb.AppendLine(line);
            }

            var bytes = Encoding.GetEncoding("Windows-1251").GetBytes(sb.ToString());
            outStream.Write(bytes, 0, bytes.Length);

        }
    }

    public interface IEgissoFactValidationError: IEgissoFact
    {
        Guid RecieverId { get; }
        int RecieverNumber { get; }

        IReadOnlyList<string> Errors { get; }
    }
}
