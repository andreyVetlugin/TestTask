using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.KpCodes;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.KpCodes
{
    public class KpCodeEditEditFormHandler : IKpCodeEditEditFormHandler
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public KpCodeEditEditFormHandler(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
        }

        public OperationResult Handle(KpCodeEditForm form, ModelStateDictionary modelState)
        {
            if (!form.Id.HasValue) return OperationResult.BuildFormError("Нет Id ");
            var kpCodeId = form.Id.Value;

            var kpCode = readDbContext.Get<KpCode>().FirstOrDefault(c => c.Id == kpCodeId);
            if (kpCode == null)
                throw new Exception("");

            writeDbContext.Attach(kpCode);
            kpCode.Code = form.KpCode;
            kpCode.Title = form.Title;
           

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
