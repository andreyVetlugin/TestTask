using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Infrastructure.Services.ExtraPays;
using AisBenefits.Models.PersonInfos;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using NPOI.XWPF.UserModel;

namespace AisBenefits.Services.Word
{
    public class PersonInfoWordModelBuilder : IPersonInfoWordModelBuilder
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public PersonInfoWordModelBuilder(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public FileResult BuildTotal(PersonInfoModel personInfoModel, WorkInfoModel workInfoModel)
        {
            var fs = new MemoryStream();
            var doc = new XWPFDocument();

            if (personInfoModel.RootId == null) throw new Exception("У этого пользователя нет рут АЙДи");
            var extraPay = readDbContext.GetExtraPayModelData(personInfoModel.RootId.Value).Data.BuildModel();
            var workInfos = workInfoModel.WorkPlaces;


            decimal perksDivPerc = 0;
            decimal vyslugaDivPerc = 0;
            decimal secrecyDivPerc = 0;
            decimal qualDivPerc = 0;
            if (extraPay.SalaryMultiplied != 0)
            {
                perksDivPerc = (extraPay.Perks / extraPay.SalaryMultiplied)*100;
                vyslugaDivPerc = (extraPay.Vysluga / extraPay.SalaryMultiplied)*100;
                secrecyDivPerc = (extraPay.Secrecy / extraPay.SalaryMultiplied)*100;
                qualDivPerc = (extraPay.Qualification / extraPay.SalaryMultiplied)*100;
            }

            



            #region Шапка

            //Дата вверху
            var paragraph = SetParagraph(doc);
            var run = paragraph.CreateRun();
            SetText(run, DateTime.Today.ToShortDateString());

            //Заголовок Регистрационная карта
            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.CENTER;
            run = paragraph.CreateRun();
            run.IsBold = true;
            SetText(run, "РЕГИСТРАЦИОННАЯ КАРТА");

            #endregion


            #region Личное инфо
            //Заголовок личного дела
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, "================== ПЕНСИОНЕРА - МУНИЦИПАЛЬНОГО СЛУЖАЩЕГО ===================");

            //Номер по реестру
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, $"Номер по реестру: {personInfoModel.Number}");

            //ФИО
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, $"Ф.И.О.: {personInfoModel.SurName} {personInfoModel.Name} {personInfoModel.MiddleName?? " "}");

            //Пол
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            var text = AddSpace($"Пол : {personInfoModel.Sex} ");
            SetText(run, text);

            //Дата Рождения
            run = paragraph.CreateRun();
            SetText(run, $"Дата рождения:  {personInfoModel.BirthDate.ToShortDateString()}");

            #endregion


            #region Адрес

            //Шапка адреса
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, "---------------------------------- Адрес -----------------------------------");

            //Адрес
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, $"{personInfoModel.Address ?? " "}");

            //Телефон
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, $"Телефоны: {personInfoModel.Phone ?? " "}");


            #endregion


            #region Паспорт

            //Заголовок паспорта
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, "--------------------------------- Паспорт ----------------------------------");

            // Паспорт Серия Номер
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = AddSpace($"{personInfoModel.DocType ?? " "} : {personInfoModel.DocSeria ?? " "} {personInfoModel.DocNumber ?? " "}");
            SetText(run, text);

            //Когда выдан
            run = paragraph.CreateRun();
            var date = !personInfoModel.IssueDate.HasValue
                ? " "
                : personInfoModel.IssueDate.Value.ToShortDateString();
            SetText(run, $"Дата выдачи: {date}");

            //Кто выдал
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, $"Кем выдан:  {personInfoModel.Issuer ?? " "}");

            //Конец раздела
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            SetText(run, "----------------------------------------------------------------------------");


            // Номер СНИЛС
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = AddSpace($"Номер СНИЛС: {personInfoModel.SNILS ?? " "}");
            SetText(run, text);

            //Район выплаты
            run = paragraph.CreateRun();
            SetText(run, $"Район выплаты: {personInfoModel.District ?? " "}");

            //Вид пенсии
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = $"Вид пенсии: {personInfoModel.PensionType}";
            SetText(run, text);

            // Номер пенсионного дела
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = AddSpace($"Номер пенсионного дела: {personInfoModel.PensionCaseNumber}");
            SetText(run, text);

            //Размер трудовой пенсии
            run = paragraph.CreateRun();
            text = $"Размер трудовой пенсии: {extraPay.GosPension.ToCommaSeparatedMoneyString()}";
            SetText(run, text);

            //Полоска
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = "----------------------------------------------------------------------------";
            SetText(run, text);



            #endregion

            #region Места работ

            var lastDate = default(DateTime);

            foreach (var workPlace in workInfos)
            {
                paragraph = SetParagraph(doc);
                run = paragraph.CreateRun();
                text = $"Организация: {workPlace.OrganizationName}";
                SetText(run, text);

                paragraph = SetParagraph(doc);
                run = paragraph.CreateRun();
                text = $"Должность: {workPlace.Function}";
                SetText(run, text);
                if (workPlace.EndDate > lastDate) lastDate = workPlace.EndDate; //Это чтобы узнать последнее увольнение
            }

            //ДАТЫ
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            var submitDate = workInfoModel.DocsSubmitDate ?? default(DateTime);
            text = $"Дата: увольнения {lastDate.ToShortDateString()}, подачи док-в {submitDate.ToShortDateString()}, " +
                   $"назначения {submitDate.ToShortDateString()}";
            SetText(run, text);

            //СТАж
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = $"Стаж: {FormatExp(workInfoModel.AgeYears, workInfoModel.AgeMonths,workInfoModel.AgeDays)}";
            SetText(run, text);

            //Пустая строка
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = $" ";
            SetText(run, text);

            #endregion

            #region Расчёты

            //Заголовок расчётов
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = $"------------------- Расчёт пенсии за выслугу лет ---------------------------";
            SetText(run, text);

            

            // Уральский коэффициент
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = FormatPay1("Уральский коэффициент:", extraPay.UralMultiplier);
            SetText(run, text);
            //Надбавки
            run = paragraph.CreateRun();
            text = FormatPay2("Надбавки (руб./%%)", extraPay.Perks, perksDivPerc);
            SetText(run, text);


            // Оклад (без ур. коэф.):
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = FormatPay1("Оклад (без ур. коэф.):", extraPay.Salary);
            SetText(run, text);
            //Выслуга
            run = paragraph.CreateRun();
            
            text = FormatPay2("Выслуга (руб./%%)", extraPay.Vysluga, vyslugaDivPerc);
            SetText(run, text);


            // Оклад (c ур. коэф.):
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = FormatPay1("Оклад (c ур. коэф.):", extraPay.SalaryMultiplied);
            SetText(run, text);
            //Секретность
            run = paragraph.CreateRun();
            text = FormatPay2("Секретность (руб./%%)", extraPay.Secrecy, secrecyDivPerc);
            SetText(run, text);


            // Премия:
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = FormatPay1("Премия:", extraPay.Premium);
            SetText(run, text);
            //Классный чин
            run = paragraph.CreateRun();
            text = FormatPay2("Классный чин (руб./%%)", extraPay.Qualification, qualDivPerc);
            SetText(run, text);


            // Матпомощь:
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = FormatPay1("Материальная помощь (МП):", extraPay.MaterialSupport);
            SetText(run, text);
            //Денежное содержание
            run = paragraph.CreateRun();
            text = FormatPay1("Денежное содержание (итог)", extraPay.Ds);
            SetText(run, text);

            //Две пустые строки
            //EMPTY
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = " ";
            SetText(run, text);
            //EMPTY
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = " ";
            SetText(run, text);



            // % от ДС (с учётом стажа)
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = $"% от ДС (с учётом стажа):......{extraPay.DsPerc} %  ";
            SetText(run, text);
            //Сумма пенсий
            run = paragraph.CreateRun();
            text = FormatPay1("Сумма пенсий:", extraPay.TotalPensionAndExtraPay);
            SetText(run, text);

            //ТИРЫ
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = "----------------------------------------------------------------------------";
            SetText(run, text);

            //EMPTY
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
            text = "";
            SetText(run, text);

            //РАЗМЕР ПЕНСИИ ЗА ВЫСЛУГУ ЛЕТ
            paragraph = SetParagraph(doc);
            paragraph.Alignment = ParagraphAlignment.CENTER;
            run = paragraph.CreateRun();
            text = personInfoModel.PayoutType == "Пенсия"
                ? $"РАЗМЕР ПЕНСИИ ЗА ВЫСЛУГУ ЛЕТ: {extraPay.TotalExtraPay}"
                : $"РАЗМЕР ДОПЛАТЫ: {extraPay.TotalExtraPay}";
            SetText(run, text);

            //ЛИНИЯ
            paragraph = SetParagraph(doc);
            run = paragraph.CreateRun();
                //text = "----------------------------------------------------------------------------";
            text = "============================================================================";
            SetText(run, text);

            #endregion






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
            run.SetFontFamily("Courier", FontCharRange.None);
            run.SetText(text);
            
            run.FontSize = 10;
        }

        private static string AddSpace(string text)
        {
            var n = 38 - text.Length;
            var res = string.Empty;
            for (var i = 0; i < n-1; i++)
            {
                res = res + " ";
            }
            
            return text + res;
        }


        public static string FormatExp(int years, int months, int days)
        {
            var y = years.ToString().TakeLast(1).FirstOrDefault();
            var m = months.ToString().TakeLast(1).FirstOrDefault();
            var d = days.ToString().TakeLast(1).FirstOrDefault();

           
            var resY = y == '1' ? "год" : ("234".Contains(y) ? "года" : "лет");
            if (years > 10 && years < 16)
                resY = "лет";
            var resM = m == '1' ? "месяц" : ("234".Contains(m) ? "месяца" : "месяцев");
            if (months > 10 && months < 13)
                resM = "месяцев";
            var resD = d == '1' ? "день" : ("234".Contains(d) ? "дня" : "дней");
            if (days > 10 && days < 16)
                resD = "дней";


            return $"{years} {resY}, {months} {resM}, {days} {resD}";


        }


        private static string FormatPay1(string name, decimal pay)
        {
            var width = 35;
            var nameLength = name.Length;
            var payStr = pay.ToString("0.00");
            var payLength = payStr.Length;
            var dotsCount = width - nameLength - payLength;
            var dots = string.Empty;

            for (int i = 0; i < dotsCount; i++)
            {
                dots = dots + ".";
            }

            if (nameLength == 26) dots = dots + "...."; // если входящий параметр - денежное содержание (костыль)

            return $"{name}{dots}{payStr}  ";

        }

        private static string FormatPay2(string name, decimal pay1, decimal pay2)
        {
            var width = 38;
            var nameLength = name.Length;
            var payStr1 = pay1.ToString("0.00");
            var payStr2 = pay2.ToString("0.00");

            var payLength1 = payStr1.Length;
            var payLength2 = payStr2.Length;

            var dotsCount = width - nameLength - payLength1 - payLength2;
            var dots = string.Empty;

            for (int i = 0; i < dotsCount; i++)
            {
                dots = dots + ".";
            }

            return $"{name}{dots}{payStr1}/{payStr2}";
        }
    }
}
