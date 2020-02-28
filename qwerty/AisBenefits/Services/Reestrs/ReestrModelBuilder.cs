using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Models.Reestrs;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Services.Reestrs
{
    public class ReestrModelBuilder : IReestrModelBuilder
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public ReestrModelBuilder(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public ReestrModel Build(Reestr reestr)
        {
            var reestrModel = Mapper.Map<Reestr, ReestrModel>(reestr);
            return reestrModel;
        }
    }
}
