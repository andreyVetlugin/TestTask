using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Misc
{
    public static class Districts
    {
        public const string ORDZHONIKIDZEVSKIY = "Орджоникидзевский";
        public const string ZHELEZNO_DOROZHNIY = "Железнодорожный";
        public const string VERKH_ISETSKIY = "Верх-Исетский";
        public const string OKTYABRSKIY = "Октябрьский";
        public const string KIROVSKIY = "Кировский";
        public const string CHKALOVSKIY = "Чкаловский";
        public const string LENINSKIY = "Ленинский";
        public const string NONE = "Не выбрано";


        public static string[] AllTypes = new[]
        {
            ORDZHONIKIDZEVSKIY,
        ZHELEZNO_DOROZHNIY,
        VERKH_ISETSKIY,
        OKTYABRSKIY,
       KIROVSKIY ,
        CHKALOVSKIY,
        LENINSKIY,
        NONE
    };
    }
}
