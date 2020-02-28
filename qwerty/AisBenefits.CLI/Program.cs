using AisBenefits.Infrastructure.Eip;
using AisBenefits.Infrastructure.Eip.Pfr.PfrBapForPeriodSmev3;
using AisBenefits.Infrastructure.Services.GosPensions;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using DataLayer.NpgSql;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AisBenefits.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var manAll = false;

            var appConfiguration = new ConfigurationBuilder()
                .AddJsonFile(@".\configuration.json")
                .Build();

            var connectionString = appConfiguration.GetSection("WebApp").Get<WebAppOptions>().GetConnectionString();
            var pfrBapConfig = appConfiguration.GetSection("WebApp").Get<WebAppOptions>().GetPfrBapForPeriodClientConfig();

            if (IsCommand(args, "database", out var databaseArgs))
            {
                if (IsCommand(databaseArgs, "update", out var updateArgs))
                {
                    Info("Накатываем миграции");
                    NpgSql.UpdateDatabase(connectionString);
                    Info("Накатили, проверяйте");
                }
                else
                {
                    goto DATABASE;
                }
            }
            else if (IsCommand(args, "gospension", out var gospensionArgs))
            {
                if (!DateTime.TryParseExact(gospensionArgs.FirstOrDefault(), "yyyy-MM", CultureInfo.CurrentCulture, DateTimeStyles.None, out var periodDate))
                    goto GOSPENSION;
                if (!ProvideOption(gospensionArgs, "-n", out var personInfoNumbers))
                    goto GOSPENSION;

                using (var dbContext = NpgSql.CreateContext(connectionString))
                {
                    var readContext = new ReadDbContext<IBenefitsEntity>(dbContext);
                    var writeContext = new WriteDbContext<IBenefitsEntity>(dbContext);

                    var personInfoRootIds = readContext.Get<PersonInfo>()
                        .ByNumbers(personInfoNumbers.Select(n => int.Parse(n)))
                        .ToList();

                    var gosPensionUpdate = personInfoRootIds.Select(p =>
                                        readContext.GetGosPensionUpdateModelData(p, periodDate)
                    ).ToList();

                    if (gosPensionUpdate.Any(r => !r.Ok))
                    {
                        Console.WriteLine("Не удалось подготовить данные для запроса для:");
                        foreach (var (r, index) in gosPensionUpdate.Select((r, i) => (r, i)))
                        {
                            if (!r.Ok)
                            {
                                Console.WriteLine($"{personInfoNumbers[index]}: {r.ErrorMessage}");
                            }
                        }

                    }

                    var client = new GosPensionPfrClient(
                    pfrBapConfig,
                    new EipLogger()
                    );

                    foreach (var gosPension in gosPensionUpdate.Where(r => r.Ok).Select(r => r.Data))
                    {
                        writeContext.ProccessGosPensionUpdate(client, gosPension);
                    }
                }
            }
            else
            {
                goto USAGE;
            }

            return;
        USAGE:
            manAll = true;
        DATABASE:
            HMan("database:");
            Man("  update: накатить миграции");
            if (!manAll) return;
        GOSPENSION:
            HMan("gospension:");
            Man("  [periodDate: yyyy-MM] [-options]");
            Man("  -n:");
            Man("    [personInfo.Number...]");
            if (!manAll) return;
        }
        static void HMan(string man)
        {
            Header();
            Man(man);
        }
        static bool usageTitle = false;
        static void Header()
        {
            if (!usageTitle)
            {
                usageTitle = true;
                Console.WriteLine();
                Console.WriteLine("  Утилита для проведения технических работ для ИС Пенсии");
                Console.WriteLine();
                Console.WriteLine("    Использование:");
                Console.WriteLine();
            }
        }
        static void Man(string man) => Console.WriteLine($"      {man}");

        static void Info(string info) => Console.WriteLine($"  {info}");

        static bool IsCommand(IEnumerable<string> args, string command, out IEnumerable<string> commandArgs)
        {
            if (args.Contains(command, StringComparer.OrdinalIgnoreCase))
            {
                commandArgs = GetArgs(args, command);
                return true;
            }

            commandArgs = new List<string>();
            return false;
        }

        static IEnumerable<string> GetArgs(IEnumerable<string> args, string command) =>
            args.SkipWhile(a => !string.Equals(a, command, StringComparison.OrdinalIgnoreCase)).Skip(1);

        static string GetOption(IEnumerable<string> args, string option) =>
            args.SkipWhile(a => !string.Equals(a, option, StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrDefault();
        static bool ProvideOption(IEnumerable<string> args, string option, out string[] values)
        {
            values = args.SkipWhile(a => !string.Equals(a, option, StringComparison.OrdinalIgnoreCase)).Skip(1).ToArray();
            return values.Length > 0;
        }
    }

    public class WebAppOptions
    {
        public string ConfigPath { get; set; }

        public string GetConnectionString()
        {
            return new ConfigurationBuilder()
                .AddJsonFile(ConfigPath)
                .Build()
                .GetConnectionString("BenefitsContext");
        }

        public PfrBapForPeriodClientConfig GetPfrBapForPeriodClientConfig()
        {
            return new ConfigurationBuilder()
                .AddJsonFile(ConfigPath)
                .Build()
                .GetSection("EipPfr:BapForPeriod").Get<PfrBapForPeriodClientConfig>();
        }
    }

    public class PfrBapForPeriodClientConfig : IPfrBapForPeriodClientConfig
    {
        public string Uri { get; set; }
        public string FrguCode { get; set; }
        public bool SignMessage { get; set; }
        public string CertificateThumbprint { get; set; }

        public int LogLevel { get; set; }
        public string LogDirectory { get; set; }

        public int Timeout { get; set; }
    }

    public class EipLogger : IEipLogger
    {
        public void Error(string context, string error)
        {
        }

        public void Info(string context, string info)
        {
        }
    }
}
