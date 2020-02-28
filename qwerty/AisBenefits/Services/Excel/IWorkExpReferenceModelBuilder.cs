using AisBenefits.Models.PersonInfos;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Services.Excel
{
    public interface IWorkExpReferenceModelBuilder
    {
        FileResult BuildTotal(PersonInfo personInfo, WorkInfoModel workInfoModel);
    }
}
