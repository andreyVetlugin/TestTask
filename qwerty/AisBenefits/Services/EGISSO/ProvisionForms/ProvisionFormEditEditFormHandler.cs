using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.ProvisionForms;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.ProvisionForms
{
    public class ProvisionFormEditEditFormHandler : IProvisionFormEditEditFormHandler
    {
        private IReadDbContext<IBenefitsEntity> readDbContext;
        private IWriteDbContext<IBenefitsEntity> writeDbContext;

        public ProvisionFormEditEditFormHandler(IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
        }

        public OperationResult Handle(ProvisionFormEditForm form, ModelStateDictionary modelState)
        {

            if (!form.Id.HasValue) return OperationResult.BuildFormError("Нет Id ");
            var provisionFormId = form.Id.Value;

            if (!modelState.IsValid)
                return OperationResult.BuildFormError("Ошибка заполнения формы");

            var provisionForm = readDbContext.Get<ProvisionForm>().FirstOrDefault(c => c.Id == provisionFormId);
            if (provisionForm == null)
                throw new SmtpException(SmtpStatusCode.BadCommandSequence);

            writeDbContext.Attach(provisionForm);
            provisionForm.Title = form.Title;
            provisionForm.Code = form.Code;



            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
