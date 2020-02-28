using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Models.PersonInfos;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace AisBenefits.Services.Excel
{
    public class WorkExpReferenceModelBuilder : IWorkExpReferenceModelBuilder
    {
        public FileResult BuildTotal(PersonInfo personInfo, WorkInfoModel workInfoModel)
        {

            var fs = new MemoryStream();
            var workbook = new XSSFWorkbook();





            ISheet sheet1 = workbook.CreateSheet("Sheet1");

            var dateFrom = workbook.CreateCellStyle();
            var dateTo = workbook.CreateCellStyle();

            var fontCell = workbook.CreateFont(); //Шрифт ячеек в таблице
            fontCell.FontName = "Times New Roman";
            fontCell.FontHeight = 14;

            var fontHead = workbook.CreateFont(); // Шрифт заголовка справки
            fontHead.FontName = "Times New Roman";
            fontHead.FontHeight = 16;

            var fontTableHead = workbook.CreateFont(); //Шрифт названий столбцов таблицы
            fontTableHead.FontName = "Times New Roman";
            fontTableHead.FontHeight = 14;
            fontTableHead.IsItalic = true;





            var cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.SetFont(fontCell);

            var cellStyle0 = workbook.CreateCellStyle();
            cellStyle0.Alignment = HorizontalAlignment.Center;
            cellStyle0.VerticalAlignment = VerticalAlignment.Center;
            cellStyle0.SetFont(fontCell);
            cellStyle0.BorderLeft = BorderStyle.Thin;
            cellStyle0.BorderRight = BorderStyle.Thin;




            var headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;
            headStyle.VerticalAlignment = VerticalAlignment.Center;
            headStyle.SetFont(fontHead);

            var tableHeadStyle = workbook.CreateCellStyle();
            tableHeadStyle.Alignment = HorizontalAlignment.Center;
            tableHeadStyle.VerticalAlignment = VerticalAlignment.Center;
            tableHeadStyle.SetFont(fontTableHead);
            tableHeadStyle.BorderBottom = BorderStyle.Thin;
            tableHeadStyle.BorderTop = BorderStyle.Thin;
            tableHeadStyle.BorderLeft = BorderStyle.Thin;
            tableHeadStyle.BorderRight = BorderStyle.Thin;


            var tableHeadStyle1 = workbook.CreateCellStyle();
            tableHeadStyle1.Alignment = HorizontalAlignment.Center;
            tableHeadStyle1.VerticalAlignment = VerticalAlignment.Center;
            tableHeadStyle1.SetFont(fontTableHead);
            tableHeadStyle1.BorderBottom = BorderStyle.Thin;
            tableHeadStyle1.BorderTop = BorderStyle.Thin;
            tableHeadStyle1.BorderRight = BorderStyle.Thin;

            var tableEndStyle = workbook.CreateCellStyle();
            tableEndStyle.Alignment = HorizontalAlignment.Center;
            tableEndStyle.VerticalAlignment = VerticalAlignment.Center;
            tableEndStyle.SetFont(fontTableHead);
            tableEndStyle.BorderTop = BorderStyle.Thin;


            IDataFormat dataFormatCustom = workbook.CreateDataFormat();


            var rowHead0 = sheet1.CreateRow(0);
            var rowHead1 = sheet1.CreateRow(1);
            var rowHead2 = sheet1.CreateRow(2);
            var rowHead3 = sheet1.CreateRow(3);

            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 4));
            sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 0, 4));
            sheet1.AddMergedRegion(new CellRangeAddress(2, 2, 0, 4));
            sheet1.AddMergedRegion(new CellRangeAddress(3, 3, 0, 4));

            //rowHead0.Height = 80;
            //rowHead1.Height = 80;
            //rowHead2.Height = 80;
            //rowHead3.Height = 80;


            rowHead0.CreateCell(0).SetCellValue("СПРАВКА о стаже");
            var h1 = rowHead0.GetCell(0).CellStyle = headStyle;
            rowHead1.CreateCell(0).SetCellValue("принятом для расчёта пенсии за выслугу лет");
            var h2 = rowHead1.GetCell(0).CellStyle = headStyle;
            rowHead2.CreateCell(0).SetCellValue("");
            rowHead3.CreateCell(0).SetCellValue($"{personInfo.SurName} {personInfo.Name} {personInfo.MiddleName ?? " "}");
            var h3 = rowHead3.GetCell(0).CellStyle = headStyle;



            //sheet1.AutoSizeColumn(0);
            sheet1.HorizontallyCenter = true;
            sheet1.VerticallyCenter = true;




            var tableHead = sheet1.CreateRow(4);

            for (int i = 0; i < 5; i++)
            {
                var currentCell = tableHead.CreateCell(i);
            }

            sheet1.AddMergedRegion(new CellRangeAddress(4, 4, 0, 1));

            var per = tableHead.GetCell(0);
            per.SetCellValue("Период работы");
            per.CellStyle = tableHeadStyle;
            var per1 = tableHead.GetCell(1);
            per1.CellStyle = tableHeadStyle1;

            var orgHead = tableHead.GetCell(2);
            orgHead.SetCellValue("Место работы");
            orgHead.CellStyle = tableHeadStyle;

            var doljHead = tableHead.GetCell(3);
            doljHead.SetCellValue("Должность");
            doljHead.CellStyle = tableHeadStyle;

            var expHead = tableHead.GetCell(4);
            expHead.CellStyle = tableHeadStyle;
            expHead.SetCellValue("Стаж");

            var currentRow = 5;

            foreach (var work in workInfoModel.WorkPlaces)
            {
                var row = sheet1.CreateRow(currentRow);

                for (int i = 0; i < 5; i++)
                {
                    var currentCell = row.CreateCell(i);
                }


                var start = row.GetCell(0);
                start.SetCellValue(work.StartDate.Date);
                start.CellStyle = cellStyle0;
                start.CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");

                var end = row.GetCell(1);
                end.SetCellValue(work.EndDate.Date);
                end.CellStyle = cellStyle0;
                end.CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");



                var org = row.GetCell(2);
                org.SetCellValue(work.OrganizationName);
                org.CellStyle = cellStyle0;

                var funct = row.GetCell(3);
                funct.SetCellValue(work.Function);
                funct.CellStyle = cellStyle0;
                var exp = row.GetCell(4);
                var months = work.AgeMonths;
                var days = work.AgeDays;
                var years = work.AgeYears;
                months = months % 12;
                exp.SetCellValue($"{years}г {months}м {days}д");
                exp.CellStyle = cellStyle0;
                currentRow++;

            }

            var lastRow = sheet1.CreateRow(currentRow);

            for (int i = 0; i < 5; i++)
            {
                var currentCell = lastRow.CreateCell(i);
            }
            sheet1.AddMergedRegion(new CellRangeAddress(currentRow, currentRow, 2, 4));



            for (int i = 0; i < 5; i++)
            {
                var temp = sheet1.GetRow(currentRow).GetCell(i);
                //sum.SetCellValue("Суммарный стаж: " + workInfoModel.Experience);
                temp.CellStyle = tableEndStyle;
            }

            var sum = sheet1.GetRow(currentRow).GetCell(2);
            sum.SetCellValue($"Суммарный стаж: {workInfoModel.AgeYears}г. {workInfoModel.AgeMonths}м. {workInfoModel.AgeDays}д.");


            //var emptyLast0 = sheet1.GetRow(currentRow).GetCell(0);
            //var emptyLast1 = sheet1.GetRow(currentRow).GetCell(1);

            //emptyLast0.CellStyle = tableEndStyle;
            //emptyLast1.CellStyle = tableEndStyle;



            sheet1.AutoSizeColumn(0);
            sheet1.AutoSizeColumn(1);
            sheet1.AutoSizeColumn(2);
            sheet1.AutoSizeColumn(3);
            sheet1.AutoSizeColumn(4);


            workbook.Write(fs, true);
            fs.Position = 0;

            return new FileStreamResult(fs,
                                   "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");


        }
    }
}
