using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace GamesManager.Core
{
    public class GamesManagerInstaller
    {
        private readonly IConfiguration configuration;
        
        public GamesManagerInstaller(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Install(IServiceCollection services) // все ли внутри из этого нужно ?
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddScoped<IReadDbContext<IDbEntity>>(sp =>
                    new ReadDbContext<IDbEntity>(sp.GetRequiredService<GamesContex>()))
                .AddScoped<IWriteDbContext<IDbEntity>>(sp =>
                    new WriteDbContext<IDbEntity>(sp.GetRequiredService<GamesContex>()))
                .AddEntityFrameworkNpgsql()
                .AddDbContext<GamesContex>();
            // 
            //.AddDbContext<BenefitsContext>(options => options.UseNpgsql(configuration.GetConnectionString("BenefitsContext")), ServiceLifetime.Scoped);

        }
    }
}
