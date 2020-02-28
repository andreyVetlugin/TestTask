﻿using AisBenefits.Models.Reestrs;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Services.Reestrs
{
    public interface IReestrModelBuilder
    {
        ReestrModel Build(Reestr reestr);
    }
}