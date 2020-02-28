using DataLayer.Misc;
using System;
using System.Collections.Generic;

namespace AisBenefits.Infrastructure.Services.DropDowns
{
    public static class AdditionalPensionTypes
    {
        public static readonly Guid None = Guid.Empty;
        public static readonly Guid Type1 = Guid.Parse("e66019e6-1527-4feb-b094-ae03ee6209e4");
        public static readonly Guid Type2 = Guid.Parse("a70b940a-112c-4dc5-a0ef-eea1cea158d8");
        public static readonly Guid Type3 = Guid.Parse("cd1fb5e8-0df5-4c68-9538-3d7f0e034841");
        public static readonly Guid Type4 = Guid.Parse("99edc69a-d868-4e4a-bd92-3b6e60e7db46");

        public static readonly Dictionary<Guid, string> additionalPensionTypes = new Dictionary<Guid, string>()
        {
            { None, "Нет" },
            { Type1, "Таможенные органы" },
            { Type2, "Прокуратура" },
            { Type3, "МО" },
            { Type4, "МВД" }
        };
    }
}
