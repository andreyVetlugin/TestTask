using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.KpCodes;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.KpCodes
{
    public class KpCodeEditCreateFormHandler : IKpCodeEditCreateFormHandler
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        public KpCodeEditCreateFormHandler(IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.writeDbContext = writeDbContext;
        }

        public OperationResult Handle(KpCodeEditForm form, ModelStateDictionary modelState)
        {

            var kpCode = new KpCode
            {
                Id = Guid.NewGuid(),
                Code = form.KpCode,
                Title = form.Title,
            };

            writeDbContext.Add(kpCode);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
