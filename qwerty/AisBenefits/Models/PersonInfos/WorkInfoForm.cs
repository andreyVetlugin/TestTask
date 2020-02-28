using AisBenefits.Attributes.Validation;
using AisBenefits.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.PersonInfos
{
    public class WorkInfoForm
    {
        [ValidGuid]
        public Guid PersonInfoId { get; set; }

        public WorkPlaceForm[] WorkPlaces { get; set; }
        



    }
}
