using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AisBenefits.Models.Reestrs;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NPOI.OpenXmlFormats.Wordprocessing;

namespace AisBenefits.Services.Reestrs
{
    public class CompleteReestrFileBuilder : ICompleteReestrFileBuilder
    {
        private readonly IConfiguration config;
        const int width = 84;
        const int col1 = 9;
        const int col2 = 23;
        const int col3 = 37;
        const int col4 = 15;

        public CompleteReestrFileBuilder(IConfiguration config)
        {
            this.config = config;
        }

        public FileResult Build(ReestrOUTPUT reestrOutput)
        {

            //var c = config.GetSection("CompleteReestrTXT");
            var month = reestrOutput.Reestr.Date.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));


            var irbal = config["IRBAL"]+"\r\n";
            var reestrNumber = config["ReestrNumber"] + "\r\n"; ;
            var orgName = config["OrganizationName"] + "\r\n"; ;
            var payDest = config["PayDestiny"]+ month + " " + reestrOutput.Reestr.Date.Year + "\r\n" ;
            var summ = config["Summ"] + reestrOutput.SummTotal.ToString("0.00", CultureInfo.GetCultureInfo("en-US")) + "\r\n";
            var probely = config["Probely"] + "\r\n";

            var text = irbal + reestrNumber + orgName + payDest + summ + probely;


            
            MemoryStream memoryStream = new MemoryStream();
            var tw = new StreamWriter(memoryStream, Encoding.GetEncoding("Windows-1251"));

            tw.Write(text);           


            foreach (var elem in reestrOutput.ReestrElements)
            {
                tw.Write(
                    string.Join("^", elem.Account, elem.SurName, elem.FirstName, elem.MiddleName, elem.Summ.ToString("0.00", CultureInfo.GetCultureInfo("en-US")), "\r\n")
                    );               
                }

            tw.Flush();
            memoryStream.Position = 0;

            return new FileStreamResult(memoryStream, "application/msword;charset=Windows-1251");
        }

        public FileResult Build(ReestrOUTPUT reestrOutput, bool forSign)
        {
            if (!forSign)
                return Build(reestrOutput);



            var date = reestrOutput.Reestr.Date.ToString("D");
            var divider = "------------------------------------------------------------------------------------\r\n";
            var empty = "\r\n";
            var head = "Название организации: Администрация г.Екатеринбурга (Вклады) \r\n " +
                       $"         Реестр №  7011  от {date} \r\n " +
                       "по зачислению денежных средств на счета международных банковских карт. \r\n" +
                       "в ОАО 'Банк'Екатеринбург', ИНН 6608005109, БИК 046577904, \r\n" +
                       "к/сч 30101810500000000904 в ГРКЦ ГУ Банка по Свердловской обл. \r\n";

            var tableHeader =
                "|   N   |    Номер карты       |        Фамилия, имя, отчество      |     Сумма    |\r\n" +
                "| п / п |                      |                                    |              |\r\n";


            


            MemoryStream memoryStream = new MemoryStream();
            var tw = new StreamWriter(memoryStream, Encoding.GetEncoding("Windows-1251"));


            tw.Write(empty);
            tw.Write(head);
            tw.Write(empty);
            tw.Write(divider);
            tw.Write(tableHeader);
            tw.Write(divider);


            var iter = 0;
            foreach (var elem in reestrOutput.ReestrElements)
            {
                var col1 = CreateFirstCol(++iter);
                var col2 = CreateCardNumberCol(elem.Account);
                var col3 = CreateFIOCol(elem.FIO.ToUpper());
                var col4 = CreateSummCol(elem.Summ.ToString("0.00", CultureInfo.GetCultureInfo("en-US")));

                var str = col1 + col2 + col3 + col4;
                tw.Write(str);
                tw.Write(divider);

            }

            var totalStr = "|                                                            Итого: |";
            var totalnum = CreateSummCol(reestrOutput.SummTotal.ToString("0.00", CultureInfo.GetCultureInfo("en-US")));
            tw.Write(totalStr + totalnum);
            tw.Write(divider);
            tw.Write(empty);
            tw.Write(" Первый заместитель\r\n" +
            "Главы Екатеринбурга: ________________________________________ А.А.Ковальчик\r\n \r\n" +
                "Начальник\r\n" +
            "Финансово - бухгалтерского управления: ________________________ И.А.Ильичева");


            tw.Flush();
            memoryStream.Position = 0;

            return new FileStreamResult(memoryStream, "application/msword;charset=Windows-1251");

        }


        private string CreateFirstCol(int number)
        {
            var numberLength = number.ToString().Length;
            var spaceNumber = col1 - 2 - numberLength;
            var sb = new StringBuilder(col1);
            sb.Append($"|{number}");

            for (int i = 0; i < spaceNumber; i++)
            {
                sb.Append(' ');
            }
            sb.Append('|');

            return sb.ToString();

        }


        private string CreateCardNumberCol(string number)
        {
            var numberLength = number.Length;
            var spaceNumber = col2 - 1 - numberLength;
            var sb = new StringBuilder(col2);
            sb.Append($"{number}");

            for (int i = 0; i < spaceNumber; i++)
            {
                sb.Append(' ');
            }
            sb.Append('|');

            return sb.ToString();

        }

        private string CreateFIOCol(string fio)
        {
            var numberLength = fio.Length;
            var spaceNumber = col3 - 1 - numberLength;
            var sb = new StringBuilder(col3);
            sb.Append($"{fio}");

            for (int i = 0; i < spaceNumber; i++)
            {
                sb.Append(' ');
            }
            sb.Append('|');

            return sb.ToString();

        }

        private string CreateSummCol(string summ)
        {
            var numberLength = summ.Length;
            var spaceNumber = col4 - 1 - numberLength;
            var sb = new StringBuilder(col4);
            

            for (int i = 0; i < spaceNumber; i++)
            {
                sb.Append(' ');
            }
            sb.Append($"{summ}");
            sb.Append("|\r\n");

            return sb.ToString();

        }


    }
}
