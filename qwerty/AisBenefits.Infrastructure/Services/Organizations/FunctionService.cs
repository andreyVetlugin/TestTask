using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Organizations
{
    public class FunctionService : IFunctionService
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly ILogBuilder logBuilder;

        public FunctionService(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext, ILogBuilder logBuilder)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
            this.logBuilder = logBuilder;
        }

        public void Create(string name)
        {
            var func = new Function
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            writeDbContext.Add(func);
            writeDbContext.SaveChanges();
        }

        public void Edit(Guid id, string name)
        {
            var func = readDbContext.Get<Function>()
                .ById(id)
                .FirstOrDefault();
            writeDbContext.Attach(func);
            func.Name = name;
            writeDbContext.SaveChanges();
        }

        public List<Function> GetAll()
        {
            return readDbContext.Get<Function>().ToList();
        }
    }
}
