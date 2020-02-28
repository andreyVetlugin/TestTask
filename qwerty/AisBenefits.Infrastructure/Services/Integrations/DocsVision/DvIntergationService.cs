using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AisBenefits.Infrastructure.DTOs;
using AisBenefits.Infrastructure.Helpers;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Integrations.DocsVision
{
    public class DvIntergationService : IDvIntergationService
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private const string NoPayout = "Выплат нет";
        private const string NoUderganiy = "Удержаний нет";

        public DvIntergationService(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public DvIntegrationDTO GetPaymentsByPersonData(string surname, string name, string patronymic, DateTime? birthdate, DateTime start, DateTime end)
        {
            /* отсев */
            var persons = readDbContext.Get<PersonInfo>()
                .Active()
                .ByFio(surname, name, patronymic)
                .OptionalByBirthDate(birthdate)
                .ToList();

            if (persons.Count > 1) throw new TooManyPeopleException();

            var root = persons.FirstOrDefault();

            if (root == null) throw new NotFoundException();

            /* после отсева */

            /* фильтр помесячно, так что... */
            start = new DateTime(start.Year, start.Month, 1);
            end = end.AddMonths(1);
            end = new DateTime(end.Year, end.Month, 1);

            var result = new DvIntegrationDTO
            {
                BirthDate = root.BirthDate.ToString("dd.MM.yyyy"),
                Udergania = NoUderganiy,
                PaymentSum = NoPayout,
                Payments = new List<DvIntegrationPaymentDTO>()
            };

            decimal paymentSumm = 0;

            var registries = readDbContext.Get<Reestr>()
                .CompletedInDateInterval(start, end)
                .ToDictionary(x => x.Id, x => x.Date);
            var payouts = readDbContext.Get<ReestrElement>()
                .ByPersonInfoRootId(root.RootId)
                .ByReestrIds(registries.Keys)
                .ToList();
            var payments = payouts.Select(p => new
                {
                    p.Summ,
                    Date = registries.TryGetValue(p.ReestrId, out var reg) ? (DateTime?) reg.Date : null
                })
                .Where(x => x.Summ != 0 && x.Date.HasValue)
                .OrderBy(x => x.Date)
                .ToList();

            if (!payments.Any()) return result;

            foreach (var p in payments)
            {
                {
                    paymentSumm += p.Summ;
                    result.Payments.Add(new DvIntegrationPaymentDTO
                    {
                        PayDate = p.Date?.ToString("MMMM yyyy г.", CultureInfo.GetCultureInfo("ru-RU")),
                        PaySum = StringifySumm(p.Summ)
                    });
                }
            }
            result.PaymentSum = StringifySumm(paymentSumm, true);

            return result;
        }

        private string StringifySumm(decimal summ, bool needStringify = false)
        {
            var c = summ.ToString("### ### ###.00").TrimStart().Split(',', '.');
            var rubles = c[0] + " руб. ";
            var kop = c.Length == 2 ? ((c[1][0] == '0' && c[1].Length == 2) ? c[1][1].ToString() : c[1]) + " коп." : "0 коп.";
            return needStringify
                ? rubles + kop + $" ({MoneyFormatter.RurPhrase(summ).Replace(" копеек", " коп.")})"
                : rubles + kop;
        }
    }
}