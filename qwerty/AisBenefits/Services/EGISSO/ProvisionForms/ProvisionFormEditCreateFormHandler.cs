using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.ProvisionForms;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.ProvisionForms
{
    public class ProvisionFormEditCreateFormHandler : IProvisionFormEditCreateFormHandler
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        public ProvisionFormEditCreateFormHandler(IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.writeDbContext = writeDbContext;
        }

        public OperationResult Handle(ProvisionFormEditForm form, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                return OperationResult.BuildFormError("Ошибка заполнения формы");
            var provisionForm = new ProvisionForm
            {
                Id = Guid.NewGuid(),
                Title = form.Title,
                Code = form.Code
            };

            writeDbContext.Add(provisionForm);


            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
