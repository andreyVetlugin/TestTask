using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Misc
{
    public static class PensionType
    {
        public const string BY_OLD_AGE = "По старости";
        public const string BY_DISABILITY = "По инвалидности";



        public static string[] AllTypes = new[]
        {
            BY_OLD_AGE,
            BY_DISABILITY,
        };
    }
}
