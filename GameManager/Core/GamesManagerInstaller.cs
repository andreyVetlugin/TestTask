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
using GamesManager.Services.Handlers.Games;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GamesManager.Services.Handlers;
using GamesManager.Infrastructure.Services;

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
                    new ReadDbContext<IDbEntity>(sp.GetRequiredService<GamesContext>()))
                .AddScoped<IWriteDbContext<IDbEntity>>(sp =>
                    new WriteDbContext<IDbEntity>(sp.GetRequiredService<GamesContext>()))
                .AddEntityFrameworkNpgsql()
                .AddDbContext<GamesContext>();

            services
                .AddScoped<IGameEditFormCreateHandler, GameEditFormCreateHandler>()
                .AddScoped<GameEditFormGetHandler>()
                .AddScoped<GameEditFormEditHandler>()
                .AddScoped<GameEditFormGetAllHandler>()
                .AddScoped<GameEditFormRemoveHandler>()
                .AddScoped<DataValidator>();
                

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // удалить

            // 
            //.AddDbContext<BenefitsContext>(options => options.UseNpgsql(configuration.GetConnectionString("BenefitsContext")), ServiceLifetime.Scoped);

        }
    }
}
