using System;
using System.Globalization;

namespace AisBenefits.Infrastructure.Helpers
{
    public static class DecimalHelper
    {
        public static string ToMoneyString(this decimal v) =>
            Math.Round(v, 2).ToString("0.00", CultureInfo.InvariantCulture);

        public static string ToCommaSeparatedMoneyString(this decimal v) =>
            Math.Round(v, 2).ToString("0.00", CultureInfo.CurrentCulture);

        public static string ToFullMoneyString(this decimal v)
        {
            var summStr = v.ToString();
            summStr = summStr == "0" ? "0,0" : summStr;
            var c = summStr.Split(',');
            var rubles = c[0] + " руб. ";
            var kop = c[1] + " коп.";
            return rubles + kop + $" ({MoneyFormatter.RurPhrase(v)})";
        }
    }
    
}
