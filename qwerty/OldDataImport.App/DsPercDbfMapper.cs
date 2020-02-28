using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AisBenefits.Infrastructure.Helpers;
using DataLayer.Entities;
using Dynamitey;
using OldDataImport.App.Entities;

namespace OldDataImport.App
{
    public static class DsPercDbfMapper
    {
        public static DsPerc Map(IMds mds)
        {
            return new DsPerc
            {
                Id = Guid.NewGuid(),
                Period = MapPeriod(mds.Period),
                AgeDays = new WorkAge(mds.Years, mds.Months, 0).AgeDays,
                Amount = mds.Mds,
                GenderType = MapGenderType(mds.Type)
            };
        }

        static DateTime MapPeriod(string period)
        {
            return Regex.Match(period, @"^\d{4}[,\.]{1}00$").Success
                ? new DateTime(int.Parse(period.Substring(0, 4)), 1, 1)
                : period == "36312,00" ? new DateTime(1999, 6, 1) : throw new InvalidOperationException();
        }

        static DsPercGenderType MapGenderType(string type)
        {
            switch (type)
            {
                case "ж":
                    return DsPercGenderType.Female;
                case "м":
                    return DsPercGenderType.Male;
                case "о":
                    return DsPercGenderType.General;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
