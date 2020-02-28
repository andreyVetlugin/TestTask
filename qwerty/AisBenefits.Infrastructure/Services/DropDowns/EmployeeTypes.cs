
using DataLayer.Misc;
using System;
using System.Collections.Generic;

namespace AisBenefits.Infrastructure.Services.DropDowns
{
    public static class EmployeeTypes
    {
        public static Guid MunicipalEmployee = Guid.Parse("aa22bcbd-f41e-4373-9cd5-43dab1c921ff");
        public static Guid Deputat = Guid.Parse("2a422a02-7995-49d7-9f67-34d56e108910");

        public static readonly Dictionary<Guid, string> employeeTypes = new Dictionary<Guid, string>()
        {
            { MunicipalEmployee, EmploymentType.MUNICIPAL_EMPLOYEE },
            { Deputat, EmploymentType.DEPUTAT },
        };

        public static readonly Dictionary<Guid, string> ShortTitles = new Dictionary<Guid, string>()
        {
            { MunicipalEmployee, "МС" },
            { Deputat, "МД" },
        };
    }
}
