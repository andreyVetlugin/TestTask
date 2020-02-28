using AisBenefits.Core;
using AisBenefits.Infrastructure.DTOs;
using AisBenefits.Models.ExtraPayVariants;
using AisBenefits.Models.PersonInfos;
using AisBenefits.Models.Reestrs;
using AisBenefits.Models.Solutions;
using AutoMapper;
using DataLayer.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using AisBenefits.Infrastructure.Services.ExtraPays;
using AisBenefits.Services;
using AisBenefits.Services.BackgroundServices;
using Serilog;

namespace AisBenefits
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            new AisBenefitsAppInstaller(configuration).Install(services);

            return services.BuildServiceProvider(true);
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var appOptions = configuration.GetSection("Application").Get<ApplicationOptions>();
            var logLevel = (LogLevel)
                Math.Min((int) LogLevel.None,
                Math.Max((int) LogLevel.Trace,
                    (int) LogLevel.None - appOptions.LogLevel)
                );
            loggerFactory.AddFile(Path.Combine(appOptions.LogDirectory, "app.log"), logLevel);

            app.UseStaticFiles();
            app.UseMvc(); 

            Core.AutoMapper.Initialize();

            new BackgroundContext(
                app.ApplicationServices,
                new BackgroundContext.ProcessState())
                .StartLifecycleProcess();
        }

        class ApplicationOptions
        {
            public int LogLevel { get; set; }
            public string LogDirectory { get; set; }
        }
    }
}
