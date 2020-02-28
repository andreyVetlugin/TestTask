using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.Logging;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Models.ExcelReport;
using AisBenefits.Services.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/[controller]")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.WatchReports)]
    public class ExcelReportController : ControllerBase
    {
        private readonly IExcelReportFilter excelReportFilter;
        private readonly IExcelReportObjectDataBuilder excelReportObjectDataBuilder;
        private readonly IExcelReportBuilder excelReportBuilder;

        public ExcelReportController(IExcelReportFilter excelReportFilter, 
                                     IExcelReportObjectDataBuilder excelReportObjectDataBuilder,
                                     IExcelReportBuilder excelReportBuilder)
        {
            this.excelReportBuilder = excelReportBuilder;
            this.excelReportFilter = excelReportFilter;
            this.excelReportObjectDataBuilder = excelReportObjectDataBuilder;
        }


        [HttpPost]
        [Route("BuildExcelReport")]
        [GetLogging]
        public IActionResult BuildExcelReport(ExcelReportForm form)
        {
            form.PIForm.Approved = new FilterItem<bool>
            {
                IsShow = form.WIForm.Approved.IsShow,
                IsFiltered = form.WIForm.Approved.IsFiltered,
                Value = form.WIForm.Approved.Value,
                FilterType = form.WIForm.Approved.FilterType
            };
            var personInfoGuids = excelReportFilter.GetPersonsRootIds(form);
            var selectors = ExcelReportFilterSelectorBuilder.Build(form);
            var dataObjects = excelReportObjectDataBuilder.Build(personInfoGuids);
            if (!dataObjects.Ok)
                return ApiModelResult.Create(dataObjects);
            var result = excelReportBuilder.BuildTotal(dataObjects.Data, selectors);

            return result;

        }

        [HttpPost]
        [Route("BuildExcelArchiveReport")]
        [GetLogging]
        public IActionResult BuildExcelArchiveReport(ExcelReportForm form)
        {
            var personInfoGuids = excelReportFilter.GetPersonsRootIds(form, true);
            var selectors = ExcelReportFilterSelectorBuilder.Build(form);
            var dataObjects = excelReportObjectDataBuilder.Build(personInfoGuids);
            if (!dataObjects.Ok)
                return new ApiResult(404);
            var result = excelReportBuilder.BuildTotal(dataObjects.Data, selectors);

            return result;

        }

    }
}