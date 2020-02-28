using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Misc
{
    public static class AdditionalPensionType
    {
        public const string TYPE1 = "Тип 1";
        public const string TYPE2 = "Тип 2";
        public const string NONE = "Не выбрано";



        public static string[] AllTypes = new[]
        {
            TYPE1,
            TYPE2,
            NONE
        };
    }
}
