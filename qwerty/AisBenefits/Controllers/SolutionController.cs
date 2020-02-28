using System;
using System.Linq;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.Logging;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.PersonInfos;
using AisBenefits.Infrastructure.Services.Solutions;
using AisBenefits.Models;
using AisBenefits.Models.Solutions;
using AisBenefits.Services.Solutions;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("/api/solutions")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePayouts)]
    public class SolutionController : ControllerBase
    {

        private readonly ISolutionService solutionService;
        private readonly ISolutionModelBuilder solutionModelBuilder;
        private readonly IPersonInfoService personInfoService;
        private readonly ISolutionWordReportBuilder solutionWordReportBuilder;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        public SolutionController(ISolutionService solutionService, ISolutionModelBuilder solutionModelBuilder, IPersonInfoService personInfoService, ISolutionWordReportBuilder solutionWordReportBuilder, IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.solutionService = solutionService;
            this.solutionModelBuilder = solutionModelBuilder;
            this.personInfoService = personInfoService;
            this.solutionWordReportBuilder = solutionWordReportBuilder;
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
        }

        [HttpPost]
        [Route("opredelit")]
        public void Opredelit(SolutionForm solutionForm)
        {
            solutionService.Opredelit(solutionForm);
        }


        [HttpPost]
        [Route("count")]
        public void Count(SolutionForm solutionForm)
        {
            solutionService.Count(solutionForm);        
        }

        [HttpPost]
        [Route("get")]
        [GetLogging]
        public IActionResult Get(GuidIdForm data)
        {
            var solutions = readDbContext.Get<Solution>()
                .ByPersonRootId(data.Id)
                .ToList();
            var pays = readDbContext.Get<ReestrElement>()
                .ByPersonInfoRootId(data.Id)
                .Select(r => r.ReestrId)
                .ToList();
            var lastPayDate = readDbContext.Get<Reestr>()
                .ByReestrIds(pays)
                .Select(r => r.Date)
                .DefaultIfEmpty()
                .Max();
            var extraPays = readDbContext.Get<ExtraPay>()
                 .ByPersonRootId(data.Id)
                 .Select(p => new
                 {
                     p.CreateDate,
                     p.OutDate,
                     p.Ds
                 })
                 .AsEnumerable()
                 .OrderBy(p => p.CreateDate)
                 .ToList();

            return new ApiResult(solutions
                .OrderByDescending(s => s.Destination)
                .ThenBy(s => s.OutdateTime.HasValue)
                .ThenByDescending(s => s.OutdateTime)
                .Select(s => new SolutionModel
            {
                AllowDelete = s.AllowDelete(lastPayDate),
                Comment = s.Comment,
                Execution = s.Execution,
                Id = s.Id,
                DS = (extraPays.FirstOrDefault(p => p.CreateDate < s.CreateTime && (!p.OutDate.HasValue || p.OutDate > s.CreateTime))?.Ds ?? s.DS / (s.DSperc / 100)).ToMoneyString(),
                Mds = s.DS.ToMoneyString(),
                    DSperc = s.DSperc,
                Destination = s.Destination,
                SolutionType_str = Solution.ConvertTypeToString(s.Type),
                TotalExtraPay = s.TotalExtraPay.ToMoneyString(),
                TotalPension = s.TotalPension.ToMoneyString()
            }));
        }

        [HttpPost]
        [Route("pause")]
        public IActionResult Pause(SolutionForm solutionForm)
        {
            solutionService.Pause(solutionForm);
            return Ok();

        }

        [HttpPost]
        [Route("resume")]
        public IActionResult Resume(SolutionForm solutionForm)
        {
            solutionService.Resume(solutionForm);
            var result = personInfoService.ResumeSolutionForPerson(solutionForm.PersonInfoRootId);
            return new ApiOperationResult(result);
        }

        [HttpPost]
        [Route("stop")]
        public void Stop(SolutionForm solutionForm)
        {
            solutionService.Stop(solutionForm);
            personInfoService.DeactivatePersonInfo(solutionForm.PersonInfoRootId);
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(GuidIdForm data)
        {
            var res = solutionService.DeleteSolution(data.Id, out bool isSolutionActive);

            return new ApiOperationResult(res);
        }



        [HttpPost]
        [Route("PrintSolution")]
        public IActionResult PrintSolution(GuidIdForm guidIdForm)
        {
            var res = solutionWordReportBuilder.BuildTotal(guidIdForm.Id); //В отличии от остальных экшнсов, тут айдишник самого солюшна
            if (!res.Ok) return ApiModelResult.Create(res);
            return res.Data;
        }




        [HttpPost]
        [Route("PrintResumeSolution")]
        public IActionResult PrintResumeSolution(GuidIdForm guidIdForm)
        {
            var res = solutionWordReportBuilder.BuildTotal(guidIdForm.Id, SolutionType.Resume);
            if (!res.Ok) return ApiModelResult.Create(res);
            return res.Data;
        }

        [HttpPost]
        [Route("PrintStopSolution")]
        public IActionResult PrintStopSolution(GuidIdForm guidIdForm)
        {
            var res = solutionWordReportBuilder.BuildTotal(guidIdForm.Id, SolutionType.Stop);
            if (!res.Ok) return ApiModelResult.Create(res);
            return res.Data;
        }

        [HttpPost]
        [Route("PrintPauseSolution")]
        public IActionResult PrintPauseSolution(GuidIdForm guidIdForm)
        {
            var res = solutionWordReportBuilder.BuildTotal(guidIdForm.Id, SolutionType.Pause);
            if (!res.Ok) return ApiModelResult.Create(res);
            return res.Data;
        }

        [HttpPost]
        [Route("PrintOpredelitSolution")]
        public IActionResult PrintOpredelitSolution(GuidIdForm guidIdForm)
        {
            var res = solutionWordReportBuilder.BuildTotal(guidIdForm.Id, SolutionType.Opredelit);
            if (!res.Ok) return ApiModelResult.Create(res);
            return res.Data;
        }

        [HttpPost]
        [Route("PrintRejectSolution")]
        public IActionResult PrintRejectSolution(GuidIdForm guidIdForm)
        {
            var res = solutionWordReportBuilder.BuildTotal(guidIdForm.Id, SolutionType.Reject);
            if (!res.Ok) return ApiModelResult.Create(res);
            return res.Data;
        }

        [HttpPost]
        [Route("edit_comment")]
        public void EditComment(EditCommentForm form)
        {
            var pay = readDbContext.Get<Solution>()
                .ById(form.Id)
                .FirstOrDefault();
            writeDbContext.Attach(pay);
            pay.Comment = form.Comment;
            writeDbContext.SaveChanges();
        }

        public class EditCommentForm
        {
            public Guid Id { get; set; } 
            public string Comment { get; set; }
        }
    }
}