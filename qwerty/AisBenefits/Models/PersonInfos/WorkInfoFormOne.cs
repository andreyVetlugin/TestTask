using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Attributes.Validation;

namespace AisBenefits.Models.PersonInfos
{
    public class WorkInfoFormOne
    {
        [ValidGuid]
        public Guid PersonInfoId { get; set; }

        public WorkPlaceForm WorkPlace { get; set; }
    }
}
