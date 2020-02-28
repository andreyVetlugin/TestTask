using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.Services.DropDowns
{
    public static class Districts
    {
        public static readonly Guid None = Guid.Empty;
        public static readonly Guid Chkalovskiy = Guid.Parse("2db14456-7fa9-4d6a-8d74-c370c91a86d5");
        public static readonly Guid Kirovskiy = Guid.Parse("8944d493-0d69-44c8-b005-39ff50147a3c");
        public static readonly Guid Leninskiy = Guid.Parse("7d9dc0db-322b-4951-b78b-9e83e251094f");
        public static readonly Guid Oktyabrskiy = Guid.Parse("a0edbba1-c355-496d-94db-c3a37341f8ba");
        public static readonly Guid Ordzhonikidzevskiy = Guid.Parse("4767b590-c768-488f-a519-06cc6bf08d5a");
        public static readonly Guid VerkhIsetskiy = Guid.Parse("2ac61d09-937d-4664-ade7-1a09549f0fc4");
        public static readonly Guid Zheleznodorozhniy = Guid.Parse("852ae2eb-4ecf-4792-a4e1-d3f28803f2a6");

        public static readonly Dictionary<Guid, string> districts = new Dictionary<Guid, string>()
        {
            { Chkalovskiy, DataLayer.Misc.Districts.CHKALOVSKIY },
            { Kirovskiy, DataLayer.Misc.Districts.KIROVSKIY },
            { Leninskiy, DataLayer.Misc.Districts.LENINSKIY },
            { Oktyabrskiy, DataLayer.Misc.Districts.OKTYABRSKIY },
            { Ordzhonikidzevskiy, DataLayer.Misc.Districts.ORDZHONIKIDZEVSKIY},
            { VerkhIsetskiy, DataLayer.Misc.Districts.VERKH_ISETSKIY },
            { Zheleznodorozhniy, DataLayer.Misc.Districts.ZHELEZNO_DOROZHNIY },
            { None, DataLayer.Misc.Districts.NONE },

        };
    }
}
