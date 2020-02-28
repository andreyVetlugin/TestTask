using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.ActionResults;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountersController : ControllerBase
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public CountersController(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        [Route("menu_counters")]
        [HttpGet]
        public IActionResult GetMenuCounters()
        {
            var date = DateTime.Now;
            var cards = readDbContext.Get<PersonInfo>().Active().Count();
            var archiveCards = readDbContext.Get<PersonInfo>().AllArchive().Count();
            var pfr = readDbContext.Get<GosPensionUpdate>().SuccessActualAt(date.Year, date.Month).NotDeclinedAndNotApproved().Count();

            return new ApiResult(new { cards, pfr, archiveCards });
        }
    }
}