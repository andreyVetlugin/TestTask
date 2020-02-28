using AisBenefits.Infrastructure.Services;
using AisBenefits.Services.Authorize;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Internal;

namespace AisBenefits.Attributes.Logging
{
    public class GetLogging : Attribute, IAuthorizationFilter
    {
        

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUserProvider = context.HttpContext.RequestServices.GetService<ICurrentUserProvider>();
            var writeDbContext = context.HttpContext.RequestServices.GetService<IWriteDbContext<IBenefitsEntity>>();

            var user = currentUserProvider.GetCurrentUser();
            var method = context.HttpContext.Request.Method;


            context.HttpContext.Request.EnableRewind();

            var body = context.HttpContext.Request.Body;
            var reqBody = string.Empty; 
            if (method=="POST")
            {
                reqBody= new StreamReader(body).ReadToEnd();
                //reqBody = GetFormAsString(context.HttpContext.Request.Form);
                body.Position = 0;
            }            
            
            var infoLog = new GetInfoLog
            {
                ActionName = context.RouteData.Values["action"].ToString(),
                ControllerName = context.RouteData.Values["controller"].ToString(),
                Date = DateTime.Now,
                Id = Guid.NewGuid(),
                PersonId = user.Id,
                RequestBody = reqBody,
                HttpMethod = context.HttpContext.Request.Method

            };

            writeDbContext.Add(infoLog);
            writeDbContext.SaveChanges();
        }                     

    }
}
