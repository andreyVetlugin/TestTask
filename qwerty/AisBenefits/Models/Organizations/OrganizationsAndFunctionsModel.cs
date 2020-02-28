using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.Organizations
{
    public class OrganizationsAndFunctionsModel
    {
        public Organization[] Organizations { get; set; }
        public Function[] Functions { get; set; }
    }
}
