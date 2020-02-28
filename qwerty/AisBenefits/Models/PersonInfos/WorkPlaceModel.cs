using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.PersonInfos
{
    public class WorkPlaceModel
    {      
        public Guid RootId { get; set; } 

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OrganizationName { get; set; }
        public string Function { get; set; }

        public Guid OrganizationId { get; set; }
        public Guid FunctionId { get; set; }

        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public int AgeDays { get; set; }

    }
}
