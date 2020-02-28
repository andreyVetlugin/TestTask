using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using AisBenefits.Core;
using AisBenefits.Services.Authorize;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;

namespace AisBenefits.App.Test.Controllers
{
    public class ControllerTestContext : ControllerContext, IDisposable
    {
        public readonly ActionContext ActionContext;
        public readonly IServiceProvider ServiceProvider;
        private readonly IServiceScope serviceScope;

        public TController GetController<TController>()
            where TController : Controller
        {
            var controller = ServiceProvider.GetService<TController>();
            controller.ControllerContext = this;
            return controller;
        }

        public void Dispose()
        {
            HttpContextAccessor.HttpContext.Response.Body.Dispose();
            serviceScope.Dispose();
        }

        public static HttpContextAccessor HttpContextAccessor = new HttpContextAccessor();
        private static ServiceCollection ServiceCollection = new ServiceCollection();
        static ControllerTestContext()
        {
            var configuration = new ConfigurationBuilder().Build();

            ServiceCollection.AddSingleton<IHttpContextAccessor>(HttpContextAccessor);
            ServiceCollection.AddSingleton(configuration);
            ServiceCollection.AddSingleton<IConfiguration>(configuration);
            ServiceCollection.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

            new AisBenefitsAppInstaller(configuration).Install(ServiceCollection);

            Core.AutoMapper.Initialize();
        }

        public ControllerTestContext(Action<IServiceCollection> serviceInstaller, ICollection<IBenefitsEntity> dbData)
        {
            ServiceCollection.Remove(
                ServiceCollection.AsEnumerable()
                    .First(d => d.ServiceType == typeof(IReadDbContext<IBenefitsEntity>))
            );
            ServiceCollection.Remove(
                ServiceCollection.AsEnumerable()
                    .First(d => d.ServiceType == typeof(IWriteDbContext<IBenefitsEntity>))
            );

            ServiceCollection.AddScoped<IReadDbContext<IBenefitsEntity>>(sp => new ReadDbContext(dbData));
            ServiceCollection.AddScoped<IWriteDbContext<IBenefitsEntity>>(sp => new WriteDbContext(dbData));

            serviceInstaller(ServiceCollection);

            HttpContext = new DefaultHttpContext();

            ServiceProvider = HttpContext.RequestServices = ServiceCollection.BuildServiceProvider();
            serviceScope = ServiceProvider.CreateScope();
            
            HttpContext.Response.Body = new MemoryStream();
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor();
            ActionContext = new ActionContext(HttpContext, new Microsoft.AspNetCore.Routing.RouteData(), ActionDescriptor);
            HttpContextAccessor.HttpContext = HttpContext;

            HttpContext.Connection.RemoteIpAddress = new IPAddress(1);

            var user = dbData.AddUserData().Get<User>().FirstOrDefault();
            HttpContext.Request.Cookies = new RequestCookieCollection(
                new Dictionary<string, string>()
                {
                    { "Authorization", ServiceProvider.GetService<IAuthorizeTokenService>().CreateToken(user) }
                }
            );
        }
    }
}
