using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AisBenefits.Infrastructure.Helpers
{
    public static class DateHelper
    {
        public static string FormatToMonth(DateTime date)
        {
            return date.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
        }

        public static int GetNumberDaysInMonth(DateTime date)
        {
            return  new DateTime(date.Year, date.Month + 1, 1).AddDays(-1).Day;
        }

        public static DateTime GetFirstDayInMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
    }
}
