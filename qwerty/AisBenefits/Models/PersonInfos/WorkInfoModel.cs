using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.PersonInfos
{
    public class WorkInfoModel
    {
        public Guid PersonInfoRootId { get; set; }
        public bool Approved { get; set; }
        public DateTime? DocsSubmitDate { get; set; }
        public DateTime? DocsDestinationDate { get; set; }
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public int AgeDays { get; set; }

        public WorkPlaceModel[] WorkPlaces { get; set; }
    }
}
