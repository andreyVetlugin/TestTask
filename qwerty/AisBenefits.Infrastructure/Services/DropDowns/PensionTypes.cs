using DataLayer.Misc;
using System;
using System.Collections.Generic;

namespace AisBenefits.Infrastructure.Services.DropDowns
{
    public static class PensionTypes
    {
        public static readonly Guid ByOldAge = Guid.Parse("0d807f8b-909f-4460-9fbd-4cab49b854c2");
        public static readonly Guid ByDisability = Guid.Parse("d666a556-46f1-4b82-a242-ba9c36a0cae0");

        public static readonly Dictionary<Guid, string> pensionTypes = new Dictionary<Guid, string>()
        {
            { ByOldAge, PensionType.BY_OLD_AGE },
            { ByDisability, PensionType.BY_DISABILITY },

        };
    }
}
