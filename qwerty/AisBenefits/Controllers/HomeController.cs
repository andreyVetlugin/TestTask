using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System;
using System.Linq;
using AisBenefits.ActionResults;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Controllers
{
    public class HomeController: Controller
    {


        [Route("/{*path}", Name = "default", Order = 100)]
        [HttpGet]
        public ActionResult Index()
        {
            return File("index.html", "text/html");
        }

        

    }
}
