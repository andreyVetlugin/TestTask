using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services.WorkInfos;
using AisBenefits.Models.ExcelReport;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.SS.UserModel.Charts;
using NPOI.XSSF.UserModel;

namespace AisBenefits.Services.Excel
{
    public class ExcelReportBuilder : IExcelReportBuilder
    {
        public FileResult BuildTotal(IEnumerable<ExcelSourceModel> dataObjects , List<TableItemSelector<ExcelSourceModel>> selectors)
        {
            var fs = new MemoryStream();
            var workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Sheet1");

            var fontCell = workbook.CreateFont(); //Шрифт ячеек в таблице
            fontCell.FontName = "Times New Roman";
            fontCell.FontHeight = 14;


            var fontTableHead = workbook.CreateFont(); //Шрифт названий столбцов таблицы
            fontTableHead.FontName = "Times New Roman";
            fontTableHead.FontHeight = 14;
            fontTableHead.IsItalic = true;

            var tableHeadStyle = workbook.CreateCellStyle();
            tableHeadStyle.Alignment = HorizontalAlignment.Center;
            tableHeadStyle.VerticalAlignment = VerticalAlignment.Center;
            tableHeadStyle.SetFont(fontTableHead);
            tableHeadStyle.BorderBottom = BorderStyle.Thin;
            tableHeadStyle.BorderTop = BorderStyle.Thin;
            tableHeadStyle.BorderLeft = BorderStyle.Thin;
            tableHeadStyle.BorderRight = BorderStyle.Thin;

            var cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.SetFont(fontCell);



            var colTitles = selectors.Select(s => s.Title);
            var rowNumber = 0;

            var row = sheet1.CreateRow(rowNumber);
            int colNumber = 0;
            foreach (var title in colTitles)
            {
                var currentCell = row.CreateCell(colNumber);
                currentCell.CellStyle = tableHeadStyle;
                currentCell.SetCellValue(title);
                colNumber++;

            }
            rowNumber++;

            foreach (var person in dataObjects)
            {
                
                row = sheet1.CreateRow(rowNumber);
                colNumber = 0;

                foreach (var sel in selectors)
                {
                    var currentCell = row.CreateCell(colNumber);
                    currentCell.CellStyle = cellStyle;
                    var r = sel.selector(person) ?? string.Empty;
                    currentCell.SetCellValue(r.ToString());
                    colNumber++;
                }

                rowNumber++;


            }


            for (var i =0; i< selectors.Count+1 ;i++)
            {
                sheet1.AutoSizeColumn(i);
            }


            workbook.Write(fs, true);
            fs.Position = 0;

            return new FileStreamResult(fs,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");


        }
    }

    
}
