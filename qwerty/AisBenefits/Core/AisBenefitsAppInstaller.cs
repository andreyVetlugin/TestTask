using System;
using System.Linq;
using System.Reflection;
using System.Text;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Controllers;
using AisBenefits.Infrastructure.Eip;
using AisBenefits.Infrastructure.Eip.Pfr.PfrBapForPeriodSmev3;
using AisBenefits.Infrastructure.Eip.Pfr.Snils;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.Authorize;
using AisBenefits.Infrastructure.Services.GosPensions;
using AisBenefits.Infrastructure.Services.Integrations.DocsVision;
using AisBenefits.Infrastructure.Services.MinExtraPays;
using AisBenefits.Infrastructure.Services.Organizations;
using AisBenefits.Infrastructure.Services.PersonInfos;
using AisBenefits.Infrastructure.Services.Reestrs;
using AisBenefits.Infrastructure.Services.Solutions;
using AisBenefits.Infrastructure.Services.UsersAndRoles;
using AisBenefits.Models.Pfr;
using AisBenefits.Services;
using AisBenefits.Services.Authorize;
using AisBenefits.Services.EGISSO.KpCodes;
using AisBenefits.Services.EGISSO.MeasureUnits;
using AisBenefits.Services.EGISSO.PeriodTypes;
using AisBenefits.Services.EGISSO.PeriodTypes.Create;
using AisBenefits.Services.EGISSO.PeriodTypes.Edit;
using AisBenefits.Services.EGISSO.Privileges;
using AisBenefits.Services.EGISSO.ProvisionForms;
using AisBenefits.Services.Excel;
using AisBenefits.Services.Pensions;
using AisBenefits.Services.PersonInfos;
using AisBenefits.Services.Reestrs;
using AisBenefits.Services.Security;
using AisBenefits.Services.Solutions;
using AisBenefits.Services.UserAndRoles;
using AisBenefits.Services.UsersAndRoles;
using AisBenefits.Services.Word;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AisBenefits.Core
{
    public class AisBenefitsAppInstaller
    {
        private readonly IConfiguration configuration;

        public AisBenefitsAppInstaller(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Install(IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatString = "dd.MM.yyyy";
                    //options.SerializerSettings.ContractResolver = new RequireObjectPropertiesContractResolver();
                });

            if (GetType().Assembly != Assembly.GetEntryAssembly()) // Для юнит тестов
            {
                mvcBuilder
                    .AddApplicationPart(GetType().Assembly)
                    .AddControllersAsServices();
            }

            services
                .AddTransient<BenefitsAppContext>()
                .AddTransient<PfrGosPensionContext>();

            services
                .AddScoped<IReadDbContext<IBenefitsEntity>>(sp => new ReadDbContext<IBenefitsEntity>(sp.GetRequiredService<BenefitsContext>()))
                .AddScoped<IWriteDbContext<IBenefitsEntity>>(sp => new WriteDbContext<IBenefitsEntity>(sp.GetRequiredService<BenefitsContext>()))
                .AddEntityFrameworkNpgsql()
                .AddDbContext<BenefitsContext>(options => options.UseNpgsql(configuration.GetConnectionString("BenefitsContext")), ServiceLifetime.Scoped);

            services
                .AddSingleton<IHashService, HashService>()
                .AddSingleton<IPasswordHasher, PasswordHasher>();

            services
                .AddScoped<IAutorizeService, AutorizeService>()
                .AddSingleton<IAuthorizeTokenService, AuthorizeTokenService>()
                .AddScoped<IUsersPermissionGetService, UsersPermissionGetService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IUserModelBuilder, UserModelBuilder>()
                .AddScoped<IRoleModelBuilder, RoleModelBuilder>()
                .AddScoped<IPersonInfoModelBuilder, PersonInfoModelBuilder>()
                .AddScoped<IPersonInfoPreviewModelBuilder, PersonInfoPreviewModelBuilder>()
                .AddScoped<IPersonInfoService, PersonInfoService>()
                .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
                .AddScoped<ILogBuilder, LogBuilder>()
                .AddScoped<IWorkInfoModelBuilder, WorkInfoModelBuilder>()
                .AddScoped<IWorkInfoService, WorkInfoService>()
                .AddScoped<IOrganizationService, OrganizationService>()
                .AddScoped<IFunctionService, FunctionService>()
                .AddScoped<IWorkExpReferenceModelBuilder, WorkExpReferenceModelBuilder>()
                .AddScoped<IPersonInfoWordModelBuilder, PersonInfoWordModelBuilder>()
                .AddScoped<ISolutionService, SolutionService>()
                .AddScoped<ISolutionModelBuilder, SolutionModelBuilder>()
                .AddScoped<IExtraPayCountService, ExtraPayCountService>()
                .AddScoped<IPersonBankCardService, PersonBankCardService>()
                .AddScoped<IGosPensionUpdateService, GosPensionUpdateService>()
                .AddScoped<IGosPensionUpdateModelBuilder, GosPensionUpdateModelBuilder>()
                .AddScoped<IReestrService, ReestrService>()
                .AddScoped<IReestrModelBuilder, ReestrModelBuilder>()
                .AddScoped<IReestrElemModelBuilder, ReestrElemModelBuilder>()
                .AddScoped<ICompleteReestrFileBuilder, CompleteReestrFileBuilder>()
                .AddScoped<IMinExtraPayService, MinExtraPayService>()
                .AddScoped<IExtraPayWordModelBuilder, ExtraPayWordModelBuilder>()
                .AddScoped<IExcelReportBuilder, ExcelReportBuilder>()
                .AddScoped<IExcelReportFilter, ExcelReportFilter>()
                .AddScoped<IExcelReportObjectDataBuilder, ExcelReportObjectDataBuilder>()
                .AddScoped<ISolutionWordReportBuilder, SolutionWordReportBuilder>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services
                .AddSingleton<IPfrBapForPeriodClientConfigProvider, PfrBapForPeriodClientConfigProvider>()
                .AddSingleton<IPfrBapForPeriodClientConfig, PfrBapForPeriodClientConfig>((sp) =>
                    sp.GetService<IPfrBapForPeriodClientConfigProvider>().Get()
                    )
                .AddSingleton<IEipLogger, EipPfrLogger>()
                .AddSingleton<IGosPensionPfrClient, GosPensionPfrClient>()
                .AddSingleton<PfrGosPensionWorkerState>()
                
                .AddSingleton<PfrSnilsService>()
                .AddSingleton<PfrSnilsClient>(sp =>
                {
                    var config = sp.GetService<IConfiguration>().GetSection("EipPfr:Snils")
                        .Get<PfrSnilsClientConfig>();
                    return new PfrSnilsClient(new Uri(config.Uri), false, null, config.FrguCode, new EipLogger(config.LogDirectory, config.LogLevel));
                });

            services
                .AddScoped<IEgissoControllerContext, EgissoControllerContext>()
                .AddSingleton((sp) =>
                    sp.GetService<IConfiguration>()
                    .GetSection("Egisso")
                    .Get<EgissoControllerContext.Options>());

            services
                .AddScoped<IPeriodTypeEditCreateFormHandler, PeriodTypeEditCreateFormHandler>()
                .AddScoped<IPeriodTypeEditEditFormHandler, PeriodTypeEditEditFormHandler>()
                .AddScoped<IPeriodTypeEditViewModelBuilder, PeriodTypeEditViewModelBuilder>()
                .AddScoped<IMeasureUnitEditEditFormHandler, MeasureUnitEditEditFormHandler>()
                .AddScoped<IMeasureUnitEditCreateFormHandler, MeasureUnitEditCreateFormHandler>()
                .AddScoped<IProvisionFormEditEditFormHandler, ProvisionFormEditEditFormHandler>()
                .AddScoped<IProvisionFormEditCreateFormHandler, ProvisionFormEditCreateFormHandler>()
                .AddScoped<IKpCodeEditEditFormHandler, KpCodeEditEditFormHandler>()
                .AddScoped<IKpCodeEditCreateFormHandler, KpCodeEditCreateFormHandler>()
                .AddScoped<IPrivilegeEditEditFormHandler, PrivilegeEditEditFormHandler>()
                .AddScoped<IPrivilegeEditCreateFormHandler, PrivilegeEditCreateFormHandler>();

            services
                .AddScoped<IDvIntergationService, DvIntergationService>();

            services.AddScoped<IDeclarationNumberProvider, DeclarationNumberProvider>();


        }

        class TestGosPensionPfrClient : IGosPensionPfrClient
        {

            public void DeleteRequest(IPfrBapForPeriodClientRequestTarget target)
            {
            }

            public PfrBapForPeriodClientRequestInitial RequestInitial(IPfrBapForPeriodClientRequestTarget target)
            {
                return new PfrBapForPeriodClientRequestInitial(true);
            }

            private int counter = 0;
            public PfrBapForPeriodClientRequestResult RequestResult(IPfrBapForPeriodClientRequestTarget target)
            {
                return new PfrBapForPeriodClientRequestResult(true, ++counter % 2 == 0, 3000);
            }

            public void Complete()
            {

            }
        }

        class EgissoControllerContext: IEgissoControllerContext
        {
            private readonly Options options;
            private readonly IReadDbContext<IBenefitsEntity> readDbContext;
            private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

            public EgissoControllerContext(Options options, IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext)
            {
                this.options = options;
                this.readDbContext = readDbContext;
                this.writeDbContext = writeDbContext;
            }

            public string ProviderCode => options.ProviderCode;

            public string OszCode => options.OszCode;

            public void Dispose()
            {
            }

            void IWriteDbContext<IBenefitsEntity>.SaveChanges()
            {
                throw new InvalidOperationException("Запрещено вызывать сохранение напрямую!");
            }

            void IWriteDbContext<IBenefitsEntity>.Add<TEntity>(TEntity entity)
            {
                writeDbContext.Add(entity);
            }

            void IWriteDbContext<IBenefitsEntity>.Attach<TEntity>(TEntity entity)
            {
                writeDbContext.Attach(entity);
            }

            IQueryable<TEntity> IReadDbContext<IBenefitsEntity>.Get<TEntity>()
            {
                return readDbContext.Get<TEntity>();
            }

            void IWriteDbContext<IBenefitsEntity>.Remove<TEntity>(TEntity entity)
            {
                writeDbContext.Remove(entity);
            }

            void IWriteDbContext<IBenefitsEntity>.RemoveRange<TEntity>(TEntity[] entityArray)
            {
                writeDbContext.RemoveRange(entityArray);
            }

            public void SaveChanges()
            {
                writeDbContext.SaveChanges();
            }

            public class Options
            {
                public string ProviderCode { get; set; }
                public string OszCode { get; set; }
            }
        }
    }
}
