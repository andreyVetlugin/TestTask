using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.ExtraPays;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Controllers
{
    [Route("/api/utility")]
    [ApiErrorHandler, ApiController]
    [PermissionFilter(RolePermissions.SuperAdministrate)]
    public class UtilityController : Controller
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public UtilityController(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        [HttpGet, Route("getpersonsfordspercvalidation")]
        public IActionResult GetPersonsForDsPercValidation()
        {
            var persons = readDbContext.Get<PersonInfo>()
                .Root()
                .ToList();
            var result = readDbContext.GetExtraPaysModelData(persons)
                .Then(r => ModelDataResult<List<(ExtraPayModelData, PersonInfo)>>.BuildSucces(
                    r.Data
                        .Where(p => p.Instance.DsPerc != (p.DsPerc?.Amount ?? 0))
                        .Select(p => (p,
                            readDbContext.Get<PersonInfo>().ById(p.Instance.PersonRootId).FirstOrDefault()))
                        .ToList()
                ));

            return ApiModelResult.Create(result, data =>
                data.Select(p => new
                {
                    Number = p.Item2.Number,
                    Fio = $"{p.Item2.SurName} {p.Item2.Name} {p.Item2.MiddleName}",
                    ImportedDsPerc = p.Item1.Instance.DsPerc,
                    DsPerc = p.Item1.DsPerc?.Amount ?? 0,
                    WorkAge = $"{ WorkAge.GetYears(p.Item1.WorkAgeDays)} лет { WorkAge.GetMonths(p.Item1.WorkAgeDays)} месяцев { WorkAge.GetDays(p.Item1.WorkAgeDays)} дней",
                    RegistrationDate = p.Item2.CreateTime
                }).ToList());
        }
    }
}
