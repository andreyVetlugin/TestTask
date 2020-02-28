using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.ExcelReport
{
    public class PayoutReportForm
    {
        public FilterItem<decimal> LastPaySumm { get; set; }
        public FilterItem<DateTime> LastPayDate { get; set; }
        public FilterItem<PersonBankCardType> BankCardType { get; set; }
        public FilterItemString BankCardNumber { get; set; }
    }
}
