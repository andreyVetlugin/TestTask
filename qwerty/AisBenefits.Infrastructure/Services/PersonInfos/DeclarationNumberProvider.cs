using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AisBenefits.Infrastructure.Services.PersonInfos
{

    public interface IDeclarationNumberProvider
    {
        int GenerateNumber();
    }


    public class DeclarationNumberProvider: IDeclarationNumberProvider
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public DeclarationNumberProvider (IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public int GenerateNumber()
        {
            var numbers = readDbContext.Get<PersonInfo>().OnlyNumbers().DefaultIfEmpty().Max();

            return numbers + 1;
        }
    }
}
