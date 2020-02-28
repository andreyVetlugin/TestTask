using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using AisBenefits.Infrastructure.Eip;
using AisBenefits.Infrastructure.Eip.Pfr.PfrBapForPeriodSmev3;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OldDataImport.App
{
    public static class GosPensionUpdateService
    {
        public static void UpdateFromPfrRequests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("./configuration.json")
                .Build()
                .Get<ImportConfig>();

            using (var dbContext = new BenefitsContext(new DbContextOptionsBuilder()
                .UseNpgsql(config.ConnectionStrings.BenefitsContext).Options))
            {
                var readDbContext = new ReadDbContext<IBenefitsEntity>(dbContext);
                var writeDbContext = new WriteDbContext<IBenefitsEntity>(dbContext);

                var date = DateTime.Now;
                var updates = readDbContext.Get<GosPensionUpdate>().SuccessActualAt(date.Year, date.Month).ToList();

                var responseRegex = new Regex(@"GET.*?AsyncRequest([a-f0-9\-]{36})_response", RegexOptions.IgnoreCase);

                using (var zip = new ZipArchive(File.OpenRead("./Data/Source/PfrRequests/pfrrequests.zip"), ZipArchiveMode.Read))
                {
                    var responseFiles = zip.Entries
                        .GroupBy(e => responseRegex.Match(e.Name).Groups[1].Value)
                        .Where(g => !string.IsNullOrWhiteSpace(g.Key))
                        .ToDictionary(g => Guid.Parse(g.Key), g => g.ToList());

                    foreach (var update in updates.Where(u => u.Amount == 0))
                    {
                        foreach (var entry in responseFiles[update.Id])
                        {
                            using (var file = new StreamReader(entry.Open()))
                            {
                                var fileContent = file.ReadToEnd();
                                if (!fileContent.StartsWith("HTTP 1.1 200"))
                                    continue;
                                var responseContent =
                                    fileContent.Substring(fileContent.IndexOf("<BapForPeriodResponse"));
                                var response =
                                    EipXmlTextSerializer.Deserialize<BapForPeriodResponseType>(responseContent);

                                bool IsGosPension(string type)
                                {
                                    return
                                        type.Contains("3.47") ||
                                        type.Contains("3.37") ||
                                        type.Contains("3.19") ||
                                        type.Contains("2.47") ||
                                        type.Contains("2.37") ||
                                        type.Contains("2.19");
                                }

                                var gospension = response.MonthlyPayment?.FirstOrDefault()?.Payment
                                                     .Where(p => IsGosPension(p.Payment.Type))
                                                     .Select(p => p.Payment.Sum)
                                                     .FirstOrDefault() ?? 0;

                               writeDbContext.Attach(update);
                                update.Amount = gospension;
                            }

                        }
                    }
                }

                writeDbContext.SaveChanges();
            }
        }

        class ImportConfig
        {
            public ConnectionStrings ConnectionStrings { get; set; }
        }

        class ConnectionStrings
        {
            public string BenefitsContext { get; set; }
        }
    }
}
