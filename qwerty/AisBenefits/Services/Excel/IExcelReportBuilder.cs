using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Models.ExcelReport;
using AisBenefits.Models.PersonInfos;
using DataLayer.Entities;

namespace AisBenefits.Services.Excel
{
    public interface IExcelReportBuilder
    {
        FileResult BuildTotal(IEnumerable<ExcelSourceModel> dataObjects,
            List<TableItemSelector<ExcelSourceModel>> selectors);
    }
}
