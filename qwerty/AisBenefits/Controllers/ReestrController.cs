using System;
using System.Linq;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.Reestrs;
using AisBenefits.Models;
using AisBenefits.Models.Reestrs;
using AisBenefits.Services.Reestrs;
using Microsoft.AspNetCore.Mvc;
using AisBenefits.Attributes.Logging;
using AisBenefits.Infrastructure.Services;

namespace AisBenefits.Controllers
{
    [Route("api/[controller]")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministarateCatalog)]
    public class ReestrController : Controller
    {
        private readonly IReestrService reestrService;
        private readonly IReestrModelBuilder reestrModelBuilder;
        private readonly IReestrElemModelBuilder reestrElemModelBuilder;
        private readonly ICompleteReestrFileBuilder completeReestrFileBuilder;

        public ReestrController(IReestrService reestrService, IReestrModelBuilder reestrModelBuilder, IReestrElemModelBuilder reestrElemModelBuilder, ICompleteReestrFileBuilder completeReestrFileBuilder)
        {
            this.reestrService = reestrService;
            this.reestrModelBuilder = reestrModelBuilder;
            this.reestrElemModelBuilder = reestrElemModelBuilder;
            this.completeReestrFileBuilder = completeReestrFileBuilder;
        }

        //[HttpPost]
        //[Route("get")]
        //[GetLogging]
        //public IActionResult Get(ReestrForm reestrForm)
        //{
        //    var reestr = reestrService.InitOrGet(reestrForm);

        //    var res = BuildReestr(reestr.Id);
        //    return new ApiResult(res);
        //}

        [HttpGet]
        [Route("get")]
        [GetLogging]
        public IActionResult Get()
        {
            var reestr = reestrService.Get();

            if (reestr == null)
                return new ApiResult(reestr);

            var res = BuildReestr(reestr.Id);
            return new ApiResult(res);
        }

        [HttpPost]
        [Route("init")]
        [GetLogging]
        public IActionResult Init(ReestrForm reestrForm)
        {
            var reestr = reestrService.Init(reestrForm);

            var res = BuildReestr(reestr.Id);
            return new ApiResult(res);
        }


        [HttpPost]
        [Route("getArchive")]
        [GetLogging]
        public IActionResult GetArchive(GetReestrArchiveForm getReestrArchiveForm)
        {
            var reestrs = reestrService.GetArchive(getReestrArchiveForm.Year, getReestrArchiveForm.Month);

            return new ApiResult(reestrs);
        }


        [HttpGet]
        [Route("getArchive")]
        [GetLogging]
        public IActionResult GetArchive()
        {
            var reestrs = reestrService.GetArchive();

            return new ApiResult(reestrs);
        }


        [HttpPost]
        [Route("getReestrElements")]
        [GetLogging]
        public IActionResult GetReestrElements(GuidIdForm guidIdForm)
        {            
            var res = BuildReestr(guidIdForm.Id);

            return new ApiResult(res);
        }



        [HttpPost]
        [Route("complete")]
        public IActionResult Complete(ReestrFileForm form)
        {
            var reestr = reestrService.Complete(form.Id);
            var res = BuildReestr(reestr.Id);
            var file = completeReestrFileBuilder.Build(res, form.ForSignature);
            return file;
        }



        [HttpPost]
        [Route("deleteElement")]
        public IActionResult DeleteElement(GuidIdForm guidIdForm)
        {
            reestrService.DeleteFromReestr(guidIdForm.Id);
            return new ApiResult(new {Message = "Ok" }, 200);
        }


        [HttpPost]
        [Route("ReCountElement")]
        public IActionResult ReCountElement(RecountReestrElementForm form)
        {
            reestrService.ReCountElementFromReestr(form);
            
            return new ApiResult(new { Message = "Ok" }, 200);
        }

        [HttpPost]
        [Route("ReCountAllElements")]
        public IActionResult ReCountAllElements(RecountReestrElementFormArray form)
        {
            reestrService.ReCountAllElementsFromReestr(form.RecountReestrElementForms);
           
            return new ApiResult(new { Message = "Ok" }, 200);
        }

        //[HttpPost]
        //[Route("SaveReestr")]
        //public IActionResult SaveReestr(GuidIdForm form)
        //{
        //    var res = reestrService.SaveReestr(form.Id);
        //    return new ApiOperationResult(res);
        //}



        private ReestrOUTPUT BuildReestr(Guid reestrId)
        {
            var reestr = reestrService.GetReestrById(reestrId);
            var elems = reestrService.GetReestrElements(reestrId);


            var reestrModel = reestrModelBuilder.Build(reestr);
            var reestrElem = reestrElemModelBuilder.Build(elems);

            var res = new ReestrOUTPUT
            {
                Reestr = reestrModel,
                ReestrElements = reestrElem,
                NumberOfElements = reestrElem.Length,
                SummTotal = reestrElem.Select(c => Math.Round(c.Summ, 2)).Sum(),
                CanComplete = reestrElem.All(e => e.Valid)
            };
            return res;
        }

    }
}