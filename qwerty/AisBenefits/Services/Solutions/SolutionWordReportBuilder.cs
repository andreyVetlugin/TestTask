using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.DropDowns;
using AisBenefits.Infrastructure.Services.ExtraPays;
using System.Text;
using System.Globalization;

namespace AisBenefits.Services.Solutions
{
    public interface ISolutionWordReportBuilder
    {
        ModelDataResult<FileResult> BuildTotal(Guid personInfoRootId, SolutionType type);
        ModelDataResult<FileResult> BuildTotal(Guid personInfoRootId);
    }

    public class SolutionWordReportBuilder: ISolutionWordReportBuilder
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public SolutionWordReportBuilder(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public ModelDataResult<FileResult> BuildTotal(Guid solutionId)
        {
            var solution = readDbContext.Get<Solution>().ById(solutionId).FirstOrDefault();

            return BuildTotal(solution.PersonInfoRootId, solution.Type);
        }


        public ModelDataResult<FileResult> BuildTotal(Guid personInfoRootId, SolutionType type)
        {

            var personInfo = readDbContext.Get<PersonInfo>().ByRootId(personInfoRootId).Select(c => new { c.Number, c.SurName, c.Name, c.MiddleName, c.PayoutTypeId }).FirstOrDefault();

            if (personInfo.PayoutTypeId == PayoutTypes.ExtraPay)
            {
                return BuildTotalExtraPay(personInfoRootId, type);
            }

            var fs = new MemoryStream();
            var doc = new XWPFDocument();

            var headText = string.Empty;
            var parText = string.Empty;
            bool rej = false;

            switch (type)
            {
                case (SolutionType.Opredelit):
                {
                    headText = "об определении размера";
                    parText = "Определить";
                    break;
                }
                case (SolutionType.Pereraschet):
                {
                    headText = "о перерасчёте размера";
                    parText = "Перерасчитать";
                    break;
                }
                case (SolutionType.Pause):
                {
                    headText = "о приостановлении выплаты";
                    parText = "Приостановить";
                    break;
                }
                case (SolutionType.Resume):
                {
                    headText = "о возобновлении выплаты";
                    parText = "Возобновить";
                    break;
                }
                case (SolutionType.Stop):
                {
                    headText = "о прекращении выплаты";
                    parText = "Прекратить";
                    break;
                }
                default:
                {
                    headText = "об отказе в перерасчёте размера";
                    parText = "Отказать в перерасчёте размера пенсии за выслугу лет";
                    rej = true;
                    break;
                }
            }

            Solution solution;

            if (!rej)
            {
                var sol = readDbContext.Get<Solution>().ByPersonRootIdAndSolutionType(personInfoRootId, type);
                if (sol.Count() < 1) return ModelDataResult<FileResult>.BuildFormError("У этого человека нет решений такого типа");

                solution = sol.OrderByDescending(c => c.Execution).FirstOrDefault();
            }
            else
            {
                solution = new Solution
                {
                    CreateTime = DateTime.Now,
                    Comment = string.Empty,
                    Execution = DateTime.Now
                };
            }

            
            

            var extraPay = readDbContext.GetExtraPayModelData(personInfoRootId).Data.BuildModel();

            string payoutTypeHeader;
            decimal addMoney;
            string addMoneyLabel;

            if (personInfo.PayoutTypeId == PayoutTypes.ExtraPay)
            {
                payoutTypeHeader = "доплаты";
                addMoney = extraPay.TotalExtraPay;
                addMoneyLabel = "доплату";
            }
            else
            {
                payoutTypeHeader = "пенсии за выслугу лет";
                //addMoney = extraPay.Vysluga;
                addMoney = extraPay.TotalExtraPay;
                addMoneyLabel = "пенсию за выслугу лет";
            }
        

            

            var paragraph = SetParagraph(doc);
            var run = paragraph.CreateRun();
            var text = solution.Destination.Date.ToString("D")+"                          ";
            SetText(run, text);
            run = paragraph.CreateRun();
            text = $" №{personInfo.Number}";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);


            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.CENTER;
            run = paragraph.CreateRun();
            run.IsBold = true;
            text = "РЕШЕНИЕ";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.CENTER;
            run = paragraph.CreateRun();
            run.IsBold = true;
            text = $"{headText} {payoutTypeHeader}";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);


            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.CENTER;
            run = paragraph.CreateRun();
            text = $"{personInfo.SurName} {personInfo.Name} {personInfo.MiddleName}";
            SetText(run, text);
            run.IsBold = true;
            run.SetUnderline(UnderlinePatterns.Thick);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = "        В соответствии с Решением Екатеринбургской городской Думы от 27.05.1997 №18/5  " +
                   "«Об утверждении Положения «О порядке установления и выплаты ежемесячной доплаты к пенсии лицам, " +
                   "замещавшим муниципальные должности, и пенсии за выслугу лет лицам, замещавшим должности " +
                   "муниципальной службы, в муниципальном образовании «город Екатеринбург» (ред. от 13.12.2016):";
            SetText(run, text);


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            var comment = (solution.Comment == string.Empty || solution.Comment==null)
                ? "__________________________________________________________________"
                : solution.Comment;

            if (type == SolutionType.Opredelit)
            {
                
                text =
                    $"    {parText} с {solution.Execution.Date.ToString("d")} к страховой пенсии по старости в размере {ConvertMoney(extraPay.TotalPension)} {addMoneyLabel} в" +
                    $" размере {ConvertMoney(addMoney)}, исходя из общей суммы пенсий в размере {ConvertMoney(extraPay.TotalPensionAndExtraPay)}, составляющем {Math.Round(solution.DSperc)} процентов месячного" +
                    $" денежного содержания.";
            }
            else
            {
                text = rej ? $"     {parText} в связи с {comment}"
                    : $"    {parText} с {solution.Execution.Date.ToString("D")} выплату пенсии за выслугу лет{(type == SolutionType.Resume ? $" в размере {ConvertMoney(solution.TotalExtraPay)}" : "")} в связи с {comment}";


            }
            SetText(run, text);


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            // text = "________________________                                                               ______________________";
            text = "Председатель";
            SetText(run, text);
            run.FontSize = 14;


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = "Комитета социальной политики	                                                    Е.В. Левина";
            SetText(run, text);
            run.FontSize = 14;





            doc.Write(fs);
            var s = new FileStreamResult(new MemoryStream(fs.ToArray()),
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            return ModelDataResult<FileResult>.BuildSucces(s);
        }

        private static XWPFParagraph SetParagraph(XWPFDocument doc)
        {
            var par = doc.CreateParagraph();

            par.Alignment = ParagraphAlignment.BOTH;
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
            var f = new NumberFormatInfo { NumberGroupSeparator = " " };
            var summStr = summ.ToString("n", f);

            summStr = summStr == "0" ? "0.0" : summStr;
            var c = summStr.Split('.');
            
            var rubles = c[0] + " руб. ";
            var kop = c[1] + " коп.";
            return rubles + kop;
        }


        private ModelDataResult<FileResult> BuildTotalExtraPay(Guid personInfoRootId, SolutionType type)
        {
            var fs = new MemoryStream();
            var doc = new XWPFDocument();

            var headText = string.Empty;
            var parText = string.Empty;
            bool rej = false;

            switch (type)
            {
                case (SolutionType.Opredelit):
                    {
                        headText = "об определении размера ежемесячной доплаты к пенсии";
                        parText = "Определить";
                        break;
                    }
                case (SolutionType.Pereraschet):
                    {
                        headText = "о перерасчёте размера ежемесячной доплаты к пенсии";
                        parText = "Перерасчитать";
                        break;
                    }
                case (SolutionType.Pause):
                    {
                        headText = "о приостановлении выплаты ежемесячной доплаты к пенсии";
                        parText = "Приостановить";
                        break;
                    }
                case (SolutionType.Resume):
                    {
                        headText = "о возобновлении ежемесячной доплаты к пенсии";
                        parText = "Возобновить";
                        break;
                    }
                case (SolutionType.Stop):
                    {
                        headText = "о прекращении выплаты ежемесячной доплаты к пенсии";
                        parText = "Прекратить";
                        break;
                    }
                default:
                    {
                        headText = "об отказе в перерасчёте размера";
                        parText = "Отказать в перерасчёте размера пенсии за выслугу лет";
                        rej = true;
                        break;
                    }
            }

            Solution solution;

            if (!rej)
            {
                var sol = readDbContext.Get<Solution>().ByPersonRootIdAndSolutionType(personInfoRootId, type);
                if (sol.Count() < 1) return ModelDataResult<FileResult>.BuildFormError("У этого человека нет решений такого типа");

                solution = sol.OrderByDescending(c => c.Execution).FirstOrDefault();
            }
            else
            {
                solution = new Solution
                {
                    CreateTime = DateTime.Now,
                    Comment = string.Empty,
                    Execution = DateTime.Now
                };
            }


            var personInfo = readDbContext.Get<PersonInfo>().ByRootId(personInfoRootId).Select(c => new { c.Number, c.SurName, c.Name, c.MiddleName, c.PayoutTypeId }).FirstOrDefault();

            var extraPay = readDbContext.GetExtraPayModelData(personInfoRootId).Data.BuildModel();

            string payoutTypeHeader;
            decimal addMoney;
            string addMoneyLabel;


                //payoutTypeHeader = "ежемесячной доплаты к пенсии";
            addMoney = extraPay.TotalExtraPay;
            addMoneyLabel = "доплату";



            var paragraph = SetParagraph(doc);
            var run = paragraph.CreateRun();
            var text = solution.Destination.Date.ToString("D") + "                          ";
            SetText(run, text);
            run = paragraph.CreateRun();
            text = $" №{personInfo.Number}";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);


            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.CENTER;
            run = paragraph.CreateRun();
            run.IsBold = true;
            text = "РЕШЕНИЕ";
            SetText(run, text);

            paragraph = SetParagraph(doc);////
            paragraph.Alignment = ParagraphAlignment.CENTER;
            run = paragraph.CreateRun();

            text = $"{headText}";
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);


            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.CENTER;
            run = paragraph.CreateRun();
            text = $"{personInfo.SurName} {personInfo.Name} {personInfo.MiddleName}";
            SetText(run, text);
            run.IsBold = true;
            run.SetUnderline(UnderlinePatterns.Thick);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            var thisDate = "27 мая 1997";
            var thisText = "О порядке установления и выплаты ежемесячной доплаты к пенсии лицам, замещавшим должности муниципальной службы и муниципальные должности в муниципальном образовании «город Екатеринбург»";



            text = $"        В соответствии с Решением Екатеринбургской городской Думы от {thisDate} №18/5 " +
                   $"«Об утверждении Положения «{thisText}:";
            SetText(run, text);

            var comment = (solution.Comment == string.Empty || solution.Comment == null)
                ? "__________________________________________________________________"
                : solution.Comment;

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();

            text = $"        {parText}"; //Определить - Перерасчитать и тд
            SetText(run, text);
            run.IsBold = true;

            if (rej)
            {
                text = $"     {parText} в связи с {comment}";
                SetText(run, text);
            }
            else
            {

                run = paragraph.CreateRun();
                text = $" c ";
                SetText(run, text);

                run = paragraph.CreateRun();
                text = $"{solution.Execution.Date.ToString("d")}"; //Дата
                SetText(run, text);
                run.SetUnderline(UnderlinePatterns.Thick);




                if (type == SolutionType.Opredelit)
                {
                    run = paragraph.CreateRun();
                    text = " к страховой пенсии по старости в размере ";
                    SetText(run, text);

                    run = paragraph.CreateRun();
                    text = $"{ConvertMoney(extraPay.TotalPension)}";
                    SetText(run, text);
                    run.SetUnderline(UnderlinePatterns.Thick);


                    run = paragraph.CreateRun();
                    text = $" {addMoneyLabel} в размере ";
                    SetText(run, text);

                    run = paragraph.CreateRun();
                    text = $"{ConvertMoney(addMoney)}";
                    SetText(run, text);
                    run.SetUnderline(UnderlinePatterns.Thick);

                    run = paragraph.CreateRun();
                    text = $", исходя из общей суммы пенсий в размере ";
                    SetText(run, text);

                    run = paragraph.CreateRun();
                    text = $"{ConvertMoney(extraPay.TotalPensionAndExtraPay)}";
                    SetText(run, text);
                    run.SetUnderline(UnderlinePatterns.Thick);

                    run = paragraph.CreateRun();
                    text = $", составляющем ";
                    SetText(run, text);

                    run = paragraph.CreateRun();
                    text = $"{solution.DSperc}";
                    SetText(run, text);
                    run.SetUnderline(UnderlinePatterns.Thick);

                    run = paragraph.CreateRun();
                    text = $"% ";
                    SetText(run, text);


                    run = paragraph.CreateRun();
                    text = $"месячного денежного содержания.";
                    SetText(run, text);
                    //text =
                    //    $" к страховой пенсии по старости в размере {ConvertMoney(extraPay.TotalPension)} {addMoneyLabel} в" +
                    //    $" размере {ConvertMoney(addMoney)}, исходя из общей суммы пенсий в размере {ConvertMoney(extraPay.TotalPensionAndExtraPay)}, составляющем {Math.Round(solution.DSperc)} процентов месячного" +
                    //    $" денежного содержания.";
                }
                else
                {
                    run = paragraph.CreateRun();
                    thisText = type == SolutionType.Resume
                        ? "ежемесячную доплату к пенсии в связи с"
                        : "выплату ежемесячной доплаты к пенсии в связи с";
                    text = $" {thisText}";

                    SetText(run, text);


                    paragraph = SetParagraph(doc);
                    run = paragraph.CreateRun();
                    text = $"{comment}";
                    SetText(run, text);

                }


            }




            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);

            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = string.Empty;
            SetText(run, text);


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            // text = "________________________                                                               ______________________";
            text = "Председатель";
            SetText(run, text);
            run.FontSize = 14;


            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = "Комитета социальной политики	                                                    Е.В. Левина";
            SetText(run, text);
            run.FontSize = 14;





            doc.Write(fs);
            var s = new FileStreamResult(new MemoryStream(fs.ToArray()),
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            return ModelDataResult<FileResult>.BuildSucces(s);
        }


    }
}
