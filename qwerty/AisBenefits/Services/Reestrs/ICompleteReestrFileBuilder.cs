using AisBenefits.Models.Reestrs;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Services.Reestrs
{
    public interface ICompleteReestrFileBuilder
    {
        FileResult Build(ReestrOUTPUT reestrOutput);
        FileResult Build(ReestrOUTPUT reestrOutput, bool forSign);
    }
}
