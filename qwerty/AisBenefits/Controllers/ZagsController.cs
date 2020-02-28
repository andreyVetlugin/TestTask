using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.PersonInfos;
using AisBenefits.Infrastructure.Services.Solutions;
using AisBenefits.Models;
using AisBenefits.Models.Solutions;
using AisBenefits.Services.Zags;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AisBenefits.Controllers
{
    [Route("/api/zags"), ApiController]
    [ApiErrorHandler, ApiModelValidation, PermissionFilter(RolePermissions.AdministratePersonInfos)]
    public class ZagsController: Controller
    {
        private readonly IReadDbContext<IBenefitsEntity> read;
        private readonly IWriteDbContext<IBenefitsEntity> write;
        private readonly IPersonInfoService personInfoService;
        private readonly ISolutionService solutionService;

        public ZagsController(IReadDbContext<IBenefitsEntity> read, IWriteDbContext<IBenefitsEntity> write, IPersonInfoService personInfoService, ISolutionService solutionService)
        {
            this.read = read;
            this.write = write;
            this.personInfoService = personInfoService;
            this.solutionService = solutionService;
        }

        [HttpPost, Route("upload")]
        public IActionResult Upload()
        {
            var file = HttpContext.Request.Form.Files[0];

            var acts = read.GetZagsStopActData(file).ToList();

            write.CreateZagsStopActData(acts);

            write.SaveChanges();

            return new ApiResult(acts.Where(a => a.Act.State == ZagsStopActState.New).Select(a => new
            {
                a.Act.Id,
                actNumber = a.Act.Number,
                deathDate = a.Act.Date,

                a.Person.Number,
                fio = string.Join(" ", a.Person.SurName, a.Person.Name, a.Person.MiddleName),
            }));
        }

        [HttpGet, Route("acts")]
        public IActionResult GetActs()
        {
            var acts = read.GetZagsStopActData();

            return new ApiResult(acts.Select(a => new
            {
                a.Act.Id,
                actNumber = a.Act.Number,
                deathDate = a.Act.Date,

                a.Person.Number,
                fio = string.Join(" ", a.Person.SurName, a.Person.Name, a.Person.MiddleName)
            }));
        }

        [HttpPost, Route("approveact")]
        public IActionResult ApproveAct(ZagsApproveActForm form)
        {
            var act = read.GetZagsStopActData(form.Id);

            write.Attach(act.Act);

            act.Act.State = ZagsStopActState.Approved;

            write.SaveChanges();

            solutionService.Stop(new SolutionForm
            {
                PersonInfoRootId = act.Act.PersonInfoRooId,
                Destination = form.DestinationDate,
                Execution = form.ExecutionDate,
                Comment = form.Comment
            });
            personInfoService.DeactivatePersonInfo(act.Act.PersonInfoRooId);


            return Ok();
        }
        [HttpPost, Route("declineact")]
        public IActionResult DeclineAct(GuidIdForm form)
        {
            var act = read.GetZagsStopActData(form.Id);

            write.Attach(act.Act);

            act.Act.State = ZagsStopActState.Declined;

            write.SaveChanges();

            return Ok();
        }
    }

    public class ZagsApproveActForm
    {
        public Guid Id { get; set; }
        public DateTime DestinationDate { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string Comment { get; set; }
    }
}
