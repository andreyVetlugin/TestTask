using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Misc
{
    public static class EmploymentType
    {
        public const string MUNICIPAL_EMPLOYEE = "Муниципальный служащий";
        public const string DEPUTAT = "Депутат городской думы";
        


        public static string[] AllTypes = new[]
        {
            MUNICIPAL_EMPLOYEE,
            DEPUTAT,            
        };
    }
}
