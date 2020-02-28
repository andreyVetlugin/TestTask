using System;
using System.Collections.Generic;
using System.Linq;
using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.Security;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using OldDataImport.App.Entities;
using OldDataImport.Infrastructure;
namespace OldDataImport.App
{
    class Program
    {
        struct CmdArguments
        {
            public readonly bool Defined;
            private readonly IEnumerable<string> args;
            public CmdArguments(bool defined, IEnumerable<string> args)
            {
                Defined = defined;
                this.args = args;
            }

            public string GetValue(string name)
            {
                return args.SkipWhile(a => a != name).Skip(1).FirstOrDefault();
            }

            public bool GetFlag(string name)
            {
                return args.Contains(name);
            }

            public CmdArguments GetArgumentsFor(string name)
            {
                var argsFor = args.SkipWhile(a => a != name);
                return new CmdArguments(argsFor.Any(), argsFor.Skip(1));
            }
        }

        static void Main(string[] args)
        {
            var cmdArgs = new CmdArguments(true, args);

            Import(cmdArgs);

            //GosPensionUpdateService.UpdateFromPfrRequests();
            //return;
            // DbfExportService.Export();
            // return;
            
        }

        static void Import(CmdArguments args)
        {
            var dropdb = args.GetFlag("-dropdb");
            var existdb = args.GetFlag("-existdb");

            if (!(dropdb || existdb))
            {
                Console.WriteLine("Требуется 1 из параметров:");
                Console.WriteLine("-dropdb : пересоздать базу");
                Console.WriteLine("-existdb : импортировать в существующую базу");
                return;
            }

            var doplats = DbfReaderHelper.ReadAll<IDoplata>("./data/source/doplata.dbf")
                .ToDictionary(d => d.NUMBER);
            var stags = DbfReaderHelper.ReadAll<IStag>("./data/source/stag.dbf");
            var organs = DbfReaderHelper.ReadAll<IOrgan>("./data/source/vocorg.dbf")
                .Where(o => !string.IsNullOrWhiteSpace(o.ORGAN));
            var dolgns = DbfReaderHelper.ReadAll<IDolg>("./data/source/vocdolg.dbf")
                .Where(d => !string.IsNullOrWhiteSpace(d.DOLGN));
            var reshenies = DbfReaderHelper.ReadAll<IReshenie>("./data/source/reshenie.dbf")
                .Select((r, i) => (reshenie: r, index: i))
                .ToList();
                //.GroupBy(r => r.NUMBER)
                //.ToDictionary(g => g.Key, g => g.ToList());
            var reestrs = DbfReaderHelper.ReadAll<IReestr>("./data/source/reestr.dbf")
                .Where(r => r.NUMBER != 0);
            var mdss = DbfReaderHelper.ReadAll<IMds>("./data/source/mds.dbf");


            foreach (var stag in stags.Where(d => string.IsNullOrWhiteSpace(d.DOLGN)))
                stag.DOLGN = "--";
            foreach (var stag in stags.Where(d => string.IsNullOrWhiteSpace(d.UCHREGD)))
                stag.UCHREGD = "--";

            var outOfOrganization = stags.Where(s => organs.All(o => !(string.Equals(o.ORGAN, s.UCHREGD, StringComparison.OrdinalIgnoreCase) && o.KOEFF == s.KOEF))).ToList();
            var outOfFunctions = stags.Where(s => dolgns.All(o => !string.Equals(o.DOLGN, s.DOLGN, StringComparison.OrdinalIgnoreCase))).ToList();

            var organizations = organs
                .Select(OrganizationDbfMapper.Map)
                .Concat(outOfOrganization.Select(OrganizationDbfMapper.Map))
                .GroupBy(o => o.OrganizationName, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(o => o.Multiplier).First(), StringComparer.OrdinalIgnoreCase);

            var functions = dolgns
                .Select(FunctionDbfMapper.Map)
                .Concat(outOfFunctions.Select(FunctionDbfMapper.Map))
                .GroupBy(o => o.Name, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault(), StringComparer.OrdinalIgnoreCase);

            var personNumberReshenies = reshenies.GroupBy(r => r.reshenie.NUMBER)
                .ToDictionary(g => g.Key, g => g.Select(r => r.reshenie));
            var personInfos = doplats.Values
                .Where(p => !string.IsNullOrWhiteSpace(p.NPR))
                .Select(p => PersonInfoDbfMapper.Map(p, personNumberReshenies.GetValueOrDefault(p.NUMBER)))
                .ToDictionary(p => p.Number);

            var dsPercs = mdss.Select(m => DsPercDbfMapper.Map(m)).ToList();

            var workInfos = stags.Select(s =>
            {
                var personInfo = personInfos[s.NUMBER];
                var organization = organizations[s.UCHREGD];
                var function = functions[s.DOLGN];

                return WorkInfoDbfMapper.Map(personInfo, organization, function, s);
            })
                .ToList();

            var solutions = reshenies.Select(r => (solution: SolutionDbfMapper.Map(personInfos[r.reshenie.NUMBER], r.reshenie), index: r.index))
                .ToList();

            foreach (var ts in solutions.GroupBy(t => t.solution.PersonInfoRootId))
            {
                var ordered = ts
                    .OrderByDescending(s => s.index)
                    .Select(s=>s.solution)
                    .ToList();
                var preview = ordered.FirstOrDefault();
                foreach (var s in ordered.Skip(1))
                {
                    s.OutdateTime = preview.CreateTime;
                    preview = s;
                }
            }

            var extraPays = reshenies
                .GroupBy(s => s.reshenie.NUMBER)
                .Select(p =>
                {
                    var personInfo = personInfos[p.Key];

                    var lastReshenie = p.OrderByDescending(r =>(r.index, r.reshenie.DT_ISPOLN)).FirstOrDefault();

                    var doplat = doplats[p.Key];

                    return ExtraPayDbFMapper.Map(personInfo, lastReshenie.reshenie, doplat);
                })
                .ToList();

            var personBankCards = reestrs
                .GroupBy(r => r.NUMBER)
                .SelectMany(g =>
                {
                    var personInfo = personInfos[g.Key];

                    return g
                        .GroupBy(gr => gr.NCARD)
                        .Select(gr => PersonBankCardMapper.Map(personInfo, gr.OrderBy(r => r.DATA).First()));
                })
                .ToDictionary(g => (g.PersonRootId, g.Number));

            foreach (var cards in personBankCards.Values.GroupBy(c => c.PersonRootId))
            {
                var ordered = cards.OrderByDescending(c => c.CreateDate).ToList();
                var lastCard = ordered.FirstOrDefault();
                foreach (var card in ordered.Skip(1))
                {
                    card.OutDate = lastCard.CreateDate;
                    lastCard = card;
                }
            }

            var nReestrs = ReestrDbfMapper.Map(reestrs)
                .ToDictionary(r => r.Number);

            var reestrElements = reestrs
                .GroupBy(r => r.NREESTR)
                .SelectMany(g =>
                {
                    var rg = nReestrs[g.Key];

                    return g.Select(gr =>
                    {
                        var personInfo = personInfos[gr.NUMBER];
                        var card = personBankCards[(personInfo.RootId, gr.NCARD)];

                        return ReestrDbfMapper.Map(rg, personInfo, card, gr);
                    });
                })
                .ToList();

            var negativeSolutionPersonInfoIds = solutions.Select(s => s.solution)
                .Where(s => !s.OutdateTime.HasValue && (s.Type == SolutionType.Pause || s.Type == SolutionType.Stop))
                .Select(s => s.PersonInfoRootId)
                .ToHashSet();
            var noCurrentYearPayPersonIds = reestrElements.GroupBy(e => e.PersonInfoRootId)
                .Where(g => g.Max(e => nReestrs.Values.First(r => r.Id == e.ReestrId).Date).Year != DateTime.Now.Year)
                .Select(g => g.Key)
                .ToHashSet();
            var withCurrentYearSulutionPersonIds = solutions.Select(s => s.solution)
                .Where(s => !s.OutdateTime.HasValue && s.Destination.Year == DateTime.Now.Year)
                .Select(s => s.PersonInfoRootId)
                .ToHashSet();
            foreach (var personInfo in personInfos.Values)
            {
                if (negativeSolutionPersonInfoIds.Contains(personInfo.RootId) ||
                    (noCurrentYearPayPersonIds.Contains(personInfo.RootId) &&
                     !withCurrentYearSulutionPersonIds.Contains(personInfo.RootId) &&
                     personInfo.CreateTime.Year < DateTime.Now.Year))
                {
                    personInfo.StoppedSolutions = true;
                }
            }
            var config = new ConfigurationBuilder()
                .AddJsonFile("./configuration.json")
                .Build()
                .Get<ImportConfig>();

            using (var dbContext = new BenefitsContext(new DbContextOptionsBuilder()
                .UseNpgsql(config.ConnectionStrings.BenefitsContext).Options))
            {
                if (dropdb)
                {
                    dbContext.Database.EnsureDeleted();
                }

                dbContext.Database.Migrate();

                var write = new WriteDbContext<IBenefitsEntity>(dbContext);

                var masterRoleId = Guid.NewGuid();
                write.Add(new Role
                {
                    Id = masterRoleId,
                    Name = "Мастер",
                    Permissions = RolePermissionsHelper.Create(RolePermissions.SuperAdministrate, RolePermissions.AdministrateRoles, RolePermissions.AdministrateUsers, RolePermissions.AdministratePersonInfos, RolePermissions.AdministarateCatalog, RolePermissions.AdministratePayouts, RolePermissions.AdministrateReestr, RolePermissions.WatchReports)
                });

                var masterUserId = Guid.Parse("A430E835-3788-4558-B572-E67F6E06270D");
                write.Add(new User
                {
                    Id = masterUserId,
                    Login = "bobber",
                    Password = PasswordHasher.GetHash("1234567890"),
                    Name = "",
                    SecondName = ""
                });
                write.Add(new RoleUserLink
                {
                    RoleId = masterRoleId,
                    UserId = masterUserId
                });

                write.Add(new MinExtraPay
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Value = 0
                });
                

                foreach (var personInfo in personInfos.Values)
                {
                    var doplata = doplats.FirstOrDefault(c => c.Key == personInfo.Number);
                    var newNumber = doplata.Value.NPR == string.Empty ? 0 : Convert.ToInt32(doplata.Value.NPR);
                    personInfo.Number = newNumber;
                    write.Add(personInfo);
                }
                    

                foreach (var workInfo in workInfos)
                    write.Add(workInfo);
                foreach (var organization in organizations.Values)
                    write.Add(organization);
                foreach (var function in functions.Values)
                    write.Add(function);
                foreach (var dsPerc in dsPercs)
                    write.Add(dsPerc);
                foreach (var solution in solutions.Select(s => s.solution))
                    write.Add(solution);
                foreach (var extraPay in extraPays)
                    write.Add(extraPay);

                foreach (var card in personBankCards.Values)
                    write.Add(card);

                foreach (var reestr in nReestrs.Values)
                    write.Add(reestr);

                foreach (var element in reestrElements)
                    write.Add(element);

                write.SaveChanges();
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
