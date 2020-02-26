using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using DataLayer;
using GamesManager.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GamesManager.Models;
using Microsoft.EntityFrameworkCore;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace GamesManager
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            new GamesManagerInstaller(configuration).Install(services);

            return services.BuildServiceProvider(true);
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    //services.AddDbContext<GamesManagerContext>(opt =>
        //      //  opt.UseInMemoryDatabase("GamesManager"));



        //    //string connection = Configuration.GetConnectionString("DefaultConnection");
            
        //    services.AddDbContext<GamesContex>();//(options =>
        //        //options.UseSqlServer(connection));
        //    //services.AddControllersWithViews();
            
            

        //    //services.AddSingleton<DbContextOptions<GamesManagerContext>>();
        //    //services.AddSingleton<GamesManagerContext>();
        //    //services.AddScoped<IRepository<Game>,EFGameRepository>();
        //    //services.AddScoped<IRepository<Publisher>,EFPublisherRepository>();
        //}

        

        // This method gets called by the runtime. Use this method to add services to the container.

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseMvc();
        }
    }
}
