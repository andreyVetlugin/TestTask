using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Infrastructure.Helpers
{
    public static class MoneyFormatter
    {

        public static string RurPhrase(decimal money)
        {
            return CurPhrase(money,"рубль", "рубля", "рублей", "копейка", "копейки", "копеек");
        }

        public static string NumPhrase(ulong value, bool isMale)
        {
            if (value == 0UL) return "Ноль";
            string[] Dek1 = { "", " од", " дв", " три", " четыре", " пять", " шесть", " семь", " восемь", " девять", " десять", " одиннадцать", " двенадцать", " тринадцать", " четырнадцать", " пятнадцать", " шестнадцать", " семнадцать", " восемнадцать", " девятнадцать" };
            string[] Dek2 = { "", "", " двадцать", " тридцать", " сорок", " пятьдесят", " шестьдесят", " семьдесят", " восемьдесят", " девяносто" };
            string[] Dek3 = { "", " сто", " двести", " триста", " четыреста", " пятьсот", " шестьсот", " семьсот", " восемьсот", " девятьсот" };
            string[] Th = { "", "", " тысяч", " миллион", " миллиард", " триллион", " квадрилион", " квинтилион" };
            string str = "";
            for (byte th = 1; value > 0; th++)
            {
                ushort gr = (ushort)(value % 1000);
                value = (value - gr) / 1000;
                if (gr > 0)
                {
                    byte d3 = (byte)((gr - gr % 100) / 100);
                    byte d1 = (byte)(gr % 10);
                    byte d2 = (byte)((gr - d3 * 100 - d1) / 10);
                    if (d2 == 1) d1 += (byte)10;
                    bool ismale = (th > 2) || ((th == 1) && isMale);
                    str = Dek3[d3] + Dek2[d2] + Dek1[d1] + EndDek1(d1, ismale) + Th[th] + EndTh(th, d1) + str;
                };
            };
            str = str.Substring(1, 1).ToUpper() + str.Substring(2);
            return str;
        }

        #region Private members
        private static string CurPhrase(decimal money,
            string word1, string word234, string wordmore,
            string sword1, string sword234, string swordmore)
        {
            money = decimal.Round(money, 2);
            decimal decintpart = decimal.Truncate(money);
            ulong intpart = decimal.ToUInt64(decintpart);
            string str = NumPhrase(intpart, true) + " ";
            byte endpart = (byte)(intpart % 100UL);
            if (endpart > 19) endpart = (byte)(endpart % 10);
            switch (endpart)
            {
                case 1: str += word1; break;
                case 2:
                case 3:
                case 4: str += word234; break;
                default: str += wordmore; break;
            }
            byte fracpart = decimal.ToByte((money - decintpart) * 100M);
            str += " " + ((fracpart < 10) ? "0" : "") + fracpart.ToString() + " ";
            if (fracpart > 19) fracpart = (byte)(fracpart % 10);
            switch (fracpart)
            {
                case 1: str += sword1; break;
                case 2:
                case 3:
                case 4: str += sword234; break;
                default: str += swordmore; break;
            };
            return str;
        }
        private static string EndTh(byte thNum, byte dek)
        {
            bool In234 = ((dek >= 2) && (dek <= 4));
            bool More4 = ((dek > 4) || (dek == 0));
            if (((thNum > 2) && In234) || ((thNum == 2) && (dek == 1))) return "а";
            else if ((thNum > 2) && More4) return "ов";
            else if ((thNum == 2) && In234) return "и";
            else return "";
        }
        private static string EndDek1(byte dek, bool isMale)
        {
            if ((dek > 2) || (dek == 0)) return "";
            else if (dek == 1)
            {
                if (isMale) return "ин";
                else return "на";
            }
            else
            {
                if (isMale) return "а";
                else return "е";
            }
        }
        #endregion
    }
}
    

