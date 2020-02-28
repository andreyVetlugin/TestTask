using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services.DropDowns;

namespace AisBenefits.Models.PersonInfos
{
    public class PersonInfoConstantList
    {
        public Dictionary<Guid,string> EmploymentTypes { get; set; }
        public Dictionary<Guid, string> AdditionalPensionTypes { get; set; }
        public Dictionary<Guid, string> PensionTypes { get; set; }
        public Dictionary<Guid, string> PayoutTypes { get; set; }
        public Dictionary<string, string> PersonDocumentTypes { get; set; }
        public Dictionary<Guid, string> Districts { get; set; }
        public int NextNumber { get; set; }
        public List<string> DocumentIssuers { get; set; }
    }
}
