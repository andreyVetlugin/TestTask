using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Models.PersonInfos;
using AisBenefits.Infrastructure.Services.ExtraPays;

namespace AisBenefits.Models.ExcelReport
{
    public class ExcelSourceModel
    {
        public PersonInfoModel PersonInfo { get; set; }
        public WorkInfoModel WorkInfo { get; set; }
        public ExtraPayModel ExtraPay { get; set; }
        public Solution Solution { get; set; }
        public PersonBankCard PersonBankCard { get; set; }
        public ExtraPayVariant ExtraPayVariant { get; set; }
        public ReestrElement LastReestrElement { get; set; }
        public DateTime LastPaymentDate { get; set; }
    }
}
