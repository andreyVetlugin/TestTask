using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AisBenefits.Infrastructure.Services.DropDowns;
using DataLayer.Entities;
using OldDataImport.App.Entities;

namespace OldDataImport.Infrastructure
{
    public static class PersonInfoDbfMapper
    {
        public static PersonInfo Map(IDoplata doplata, IEnumerable<IReshenie> reshenies)
        {
            var personInfoId = Guid.NewGuid();

            return new PersonInfo
            {
                CreateTime = reshenies != null ? reshenies.Where(r => r.DT_ISPOLN > new DateTime(1945, 5, 9)).OrderBy(r => r.DT_ISPOLN).First().DT_ISPOLN : (doplata.DATA_PRIEM == DateTime.MinValue ? DateTime.Now : doplata.DATA_PRIEM),
                OutdateTime = null,

                Id = personInfoId,
                RootId = personInfoId,

                Number = doplata.NUMBER,
                PensionCaseNumber = doplata.NPD,

                MiddleName = doplata.OT,
                Name = doplata.IM,
                SurName = doplata.FM,
                Sex = MapSex(doplata.POL),
                BirthDate = doplata.DTR,

                DocTypeId = DocumentTypes.PASSPORT,
                DocSeria = doplata.PSR.Replace(" ", ""),
                DocNumber = doplata.PNM,
                IssueDate = doplata.PSPDATA,
                Issuer = doplata.PSPKEM,

                SNILS = MapSnils(doplata.SNILS),

                Address =
                    $"{(string.IsNullOrWhiteSpace(doplata.PINDEX) ? ("") : ($"{doplata.PINDEX}, "))}{doplata.TIPNSP} {doplata.NSPNAME}, {doplata.TIPULICA} {doplata.ULCNAME}, д. {doplata.DOM}{(string.IsNullOrWhiteSpace(doplata.KORPUS) ? "" : $"к. {doplata.KORPUS}")}{(string.IsNullOrWhiteSpace(doplata.KVARTIRA) ? "" : $", кв. {doplata.KVARTIRA}")}",
                EmployeeTypeId = MapEmployType(doplata.DOLGN),

                DistrictId = MapDistrict(doplata.RAINAMEPEN),

                Birthplace = doplata.MESTOROGD,

                PensionTypeId = MapPensionType(doplata.VDP_NAME),

                PayoutTypeId = MapPayoutType(doplata.TIP),

                AdditionalPensionId = MapAdditionalPension(doplata.VDP_NAME),

                Approved = doplata.UTV_STAGA,

                Phone = string.IsNullOrWhiteSpace(doplata.PHONEDOM) ? doplata.PHONERAB : doplata.PHONEDOM,

                Email = null,

                StoppedSolutions = MapStoppedSolutions(doplata.DELO),
            };

        }

        static readonly Regex NumberRegex = new Regex(@"^\d+$");

        static bool MapStoppedSolutions(string delo)
        {
            return delo == "Арх";
        }

        static readonly StringBuilder sb = new StringBuilder();
        static string MapSnils(string snils)
        {
            if (string.IsNullOrWhiteSpace(snils))
                return null;

            sb.Clear();
            sb.Append(snils);
            sb[sb.Length - 3] = ' ';

            return sb.ToString();
        }

        static Guid MapAdditionalPension(string vdp_name)
        {
            if (vdp_name.Contains("Прокура", StringComparison.OrdinalIgnoreCase))
                return AdditionalPensionTypes.Type2;
            if (vdp_name.Contains("оборон", StringComparison.OrdinalIgnoreCase) ||
                vdp_name.Contains("МО", StringComparison.OrdinalIgnoreCase))
                return AdditionalPensionTypes.Type3;
            if (vdp_name.Contains("ВД", StringComparison.OrdinalIgnoreCase) ||
                vdp_name.Contains("МВД", StringComparison.OrdinalIgnoreCase))
                return AdditionalPensionTypes.Type4;
            

            return AdditionalPensionTypes.None;
        }

        static Guid MapPayoutType(string tip)
        {
            switch (tip)
            {
                case "1":
                    return PayoutTypes.ExtraPay;
                case "2":
                    return PayoutTypes.Pension;
                default:
                    return PayoutTypes.ExtraPay;
            }
        }

        static Guid MapPensionType(string vdp_name)
        {
            if (vdp_name.Contains("возраст", StringComparison.OrdinalIgnoreCase))
                return PensionTypes.ByOldAge;
            if (vdp_name.Contains("инвалидност", StringComparison.OrdinalIgnoreCase))
                return PensionTypes.ByDisability;

            return PensionTypes.ByOldAge;
        }

        static Guid MapDistrict(string rainamepen)
        {
            switch (rainamepen)
            {
                case DataLayer.Misc.Districts.CHKALOVSKIY:
                    return Districts.Chkalovskiy;
                case DataLayer.Misc.Districts.KIROVSKIY:
                    return Districts.Kirovskiy;
                case DataLayer.Misc.Districts.LENINSKIY:
                case "Ленинский (Ек-бург)":
                    return Districts.Leninskiy;
                case DataLayer.Misc.Districts.OKTYABRSKIY:
                    return Districts.Oktyabrskiy;
                case DataLayer.Misc.Districts.ORDZHONIKIDZEVSKIY:
                    return Districts.Ordzhonikidzevskiy;
                case DataLayer.Misc.Districts.VERKH_ISETSKIY:
                    return Districts.VerkhIsetskiy;
                case DataLayer.Misc.Districts.ZHELEZNO_DOROZHNIY:
                    return Districts.Zheleznodorozhniy;
                default:
                    return Guid.Empty;
            }
        }

        static Guid MapEmployType(string dolgn)
        {
            return dolgn.Contains("Депут", StringComparison.OrdinalIgnoreCase)
                ? EmployeeTypes.Deputat
                : EmployeeTypes.MunicipalEmployee;
        }

        static char MapSex(string pol)
        {
            switch (pol)
            {
                case "М":
                case "м":
                    return 'М';
                default:
                    return 'Ж';
            }
        }
    }
}
