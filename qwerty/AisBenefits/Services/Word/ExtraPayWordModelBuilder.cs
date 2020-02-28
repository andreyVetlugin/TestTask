using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Models.PersonInfos;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.Util;
using NPOI.XWPF.UserModel;

namespace AisBenefits.Services.Word
{
    public class ExtraPayWordModelBuilder : IExtraPayWordModelBuilder
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public ExtraPayWordModelBuilder(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public FileResult BuildTotal(Guid personInfoRootId, DateTime from, DateTime to)
        {
            var fs = new MemoryStream();
            var doc = new XWPFDocument();


            var img = "./Img/Herb.png";//Environment.CurrentDirectory.ToString();
            var body =
                "     В соответствии с решением Екатеринбургской городской Думы от 27 мая  1997 г. № 18/5 " +
                "«Об утверждении Положения «О порядке установления и выплаты ежемесячной доплаты к пенсии лицам, " +
                "замещавшим должности муниципальной службы и муниципальные должности в муниципальном " +
                "образовании «город Екатеринбург»  установлена ежемесячная доплата к пенсии. ";

            var nameInFoot = "Е.В. Левина";

            var fromRed = new DateTime(from.Year, from.Month, 1);
            var toRed = new DateTime(to.Year, to.Month, 1);
            if (to.Year < 9999)
                toRed = toRed.AddMonths(1);

            var personInfo = readDbContext.Get<PersonInfo>().ByRootId(personInfoRootId).FirstOrDefault();

            var reestrs = readDbContext.Get<Reestr>().CompletedInDateInterval(fromRed, toRed).ToList();

            var reestrIds = reestrs.Select(c => c.Id).ToList();

            //ПОлучаем элементы реестра для теущего чувака за этот период
            var elements = readDbContext.Get<ReestrElement>()
                .ByPersonInfoRootIdAndReestrIds(personInfoRootId, reestrIds)
                .ToList();

            var elems = (from el in elements
                         join r in reestrs on el.ReestrId equals r.Id
                         select new { ReestrId = r.Id, ReestrDate = r.Date, Element = el }).OrderBy(c => c.ReestrDate)
                         .ToList();

            var paragraph = SetParagraph(doc);
            var run = paragraph.CreateRun();
            SetText(run, "                 ");
            run = paragraph.CreateRun();

            using (FileStream picFile = new FileStream(img, FileMode.Open, FileAccess.Read))
            {
                run.AddPicture(picFile, (int)PictureType.PNG, "Herb.png", Units.ToEMU(50), Units.ToEMU(50));
                var docPr = ((NPOI.OpenXmlFormats.Dml.WordProcessing.CT_Drawing)run.GetCTR().Items[0]).inline[0].docPr;
                docPr.id = 1000;
            }

            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.DISTRIBUTE;
            run = paragraph.CreateRun();
            SetText(run, "     АДМИНИСТРАЦИЯ                     ");
            run.IsBold = true;
            run = paragraph.CreateRun();
            SetText(run, "По месту требования");


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, "ГОРОДА ЕКАТЕРИНБУРГА");
            run.IsBold = true;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, "                   КОМИТЕТ");
            run.IsBold = true;
            run.FontSize = 12;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, "     СОЦИАЛЬНОЙ ПОЛИТИКИ");
            run.FontSize = 12;
            run.IsBold = true;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, " ");


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            var text =
                "       ул. 8 Марта, 8б, г. Екатеринбург, 620014";
            SetText(run, text);
            run.FontSize = 9;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
               "                Тел.371-23-38 факс 371-23-38";
            SetText(run, text);
            run.FontSize = 9;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
               "                  E-mail: ksp@adm-ekburg.ru";
            SetText(run, text);
            run.FontSize = 9;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "                 http://www.екатеринбург.рф";
            SetText(run, text);
            run.FontSize = 9;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "   ";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "______________№______________";
            SetText(run, text);
            run.FontSize = 12;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "   ";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "На №_________от______________";
            SetText(run, text);
            run.FontSize = 12;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "   ";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "О доплате к пенсии.";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "   ";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "   ";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.CENTER;
            run = paragraph.CreateRun();
            text =
                "Справка";
            SetText(run, text);
            run.IsBold = true;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "   ";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.BOTH;
            run = paragraph.CreateRun();
            text =
                body;
            SetText(run, text);


            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.CENTER;
            run = paragraph.CreateRun();
            text =
                $"{personInfo.SurName} {personInfo.Name} {personInfo.MiddleName}";
            SetText(run, text);
            run.IsBold = true;
            run.SetUnderline(UnderlinePatterns.Single);


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "   ";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            var term = EditMonthsEnding(elems.Count());
            text =
                $"     Размер доплаты за последние {term} составил:";
            SetText(run, text);


            foreach (var elem in elems)
            {
                paragraph = SetParagraph(doc);

                run = paragraph.CreateRun();
                text =
                    $"{FormatMonth(elem.ReestrDate)}";
                SetText(run, text);
                run.AddTab();

                run = paragraph.CreateRun();
                text = $" {elem.ReestrDate.Year} г.     -    {ConvertMoney(Math.Round(elem.Element.Summ, 2))}  ";
                SetText(run, text);

            }
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "   ";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            var overallMoney = elems.Select(c => Math.Round(c.Element.Summ, 2)).Sum();

            text =
                $"Доход за последние {term} составил {overallMoney.ToFullMoneyString()}.  ";
            SetText(run, text);


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "     Удержаний нет.";
            SetText(run, text);



            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                " ";
            SetText(run, text);


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "Председатель Комитета                                                       ";
            SetText(run, text);
            run = paragraph.CreateRun();
            text =
                nameInFoot;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text =
                "социальной политики";
            SetText(run, text);












            doc.Write(fs);

            return new FileStreamResult(new MemoryStream(fs.ToArray()),
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }


        private static XWPFParagraph SetParagraph(XWPFDocument doc)
        {
            var par = doc.CreateParagraph();

            par.Alignment = ParagraphAlignment.LEFT;
            par.SpacingAfterLines = 0;
            return par;
        }


        private static void SetText(XWPFRun run, string text)
        {
            run.SetFontFamily("Times New Roman", FontCharRange.None);
            run.SetText(text);

            run.FontSize = 14;
        }

        private static string ConvertMoney(decimal summ)
        {
            var summStr = summ.ToString();
            summStr = summStr == "0" ? "0,0" : summStr;
            var c = summStr.Split(',');
            var rubles = c[0] + " руб. ";
            var kop = c[1] + " коп.";
            return rubles + kop;
        }

        private static string FormatMonth(DateTime date)
        {
            var width = 14;
            var dateStr = date.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));



            var spacesCount = width - dateStr.Length;
            var spaces = string.Empty;

            for (var i = 0; i < spacesCount; i++)
            {
                spaces += " ";
            }

            return dateStr + spaces;
        }

        private static string EditMonthsEnding(int months)
        {
            var m = months.ToString().TakeLast(1).FirstOrDefault();

            var resM = m == '1' ? "месяц" : ("234".Contains(m) ? "месяца" : "месяцев");
            if (months > 10 && months < 13)
                resM = "месяцев";

            return $"{months} {resM}";


        }




    }
}
