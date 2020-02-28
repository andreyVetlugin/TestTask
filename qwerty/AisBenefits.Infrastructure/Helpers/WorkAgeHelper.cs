using System;

namespace AisBenefits.Infrastructure.Helpers
{
    public struct WorkAge
    {
        const int yearsToDays = monthsToDays * yearsToMonths;
        const int yearsToMonths = 12;
        const int monthsToDays = 30;

        public readonly int AgeDays;
        public int Years => GetYears(AgeDays);
        public int Months => GetMonths(AgeDays);
        public int Days => GetDays(AgeDays);

        public WorkAge(DateTime start, DateTime end)
        {
            AgeDays = GetAgeDays(start, end);
        }

        public WorkAge(int years, int months, int days)
        {
            AgeDays = GetAgeDays(years, months, days);
        }

        public override string ToString()
        {
            return $"{Years.ToString("00")}/{Months.ToString("00")}/{Days.ToString("00")}";
        }

        public static int GetYears(int ageDays) => ageDays / yearsToDays;
        public static int GetMonths(int ageDays) => ageDays / monthsToDays % yearsToMonths;
        public static int GetDays(int ageDays) => ageDays % monthsToDays;

        public static int GetAgeDays(DateTime start, DateTime end)
        {
            return GetAgeDays(end.Year - start.Year, end.Month - start.Month, end.Day - start.Day);
        }

        public static int GetAgeDays(int years, int months, int days)
        {
            return years * yearsToDays + months * monthsToDays + days + 1;
        }

        public static WorkAge operator +(WorkAge lv, WorkAge rv)
        {
            return new WorkAge(lv.AgeDays + rv.AgeDays);
        }

        public static WorkAge operator -(WorkAge lv, WorkAge rv)
        {
            return new WorkAge(lv.AgeDays - rv.AgeDays);
        }

        public static WorkAge operator *(WorkAge lv, decimal rv)
        {
            return new WorkAge((int)(lv.AgeDays * rv));
        }

        public WorkAge(int ageDays)
        {
            AgeDays = ageDays;
        }
    }
}
