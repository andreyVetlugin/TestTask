using System;
using System.Collections.Generic;

namespace AisBenefits.Infrastructure.Services.DropDowns
{
    public static class PayoutTypes
    {
        public static readonly Guid Pension = Guid.Parse("0235a5e2-fe16-4967-bb8a-dd91e9bb76bd");
        public static readonly Guid ExtraPay = Guid.Parse("045c8ef4-b83c-46d8-9b48-60eb9119714a");

        public static readonly Dictionary<Guid, string> payoutTypes = new Dictionary<Guid, string>()
        {
            { Pension, "Пенсия"},
            { ExtraPay, "Доплата"}
        };
    }

    public class PayoutType
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
