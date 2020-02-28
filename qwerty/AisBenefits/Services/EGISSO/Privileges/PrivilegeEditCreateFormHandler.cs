using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.Privileges;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.Privileges
{
    public class PrivilegeEditCreateFormHandler : IPrivilegeEditCreateFormHandler
    {

        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        public PrivilegeEditCreateFormHandler(IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.writeDbContext = writeDbContext;
        }

        public OperationResult Handle(PrivilegeEditForm form, ModelStateDictionary modelState)
        {
           
            CreatePrivilege(form);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        private void CreatePrivilege(PrivilegeEditForm form)
        {
            var privilege = new Privilege
            {
                Id = Guid.NewGuid(),
                Title = form.Title,
                EgissoCode = form.EgissoCode,
                PeriodTypeId = form.PeriodTypeId,
                ProvisionFormId = form.ProvisionFormId,
                UsingNeedCriteria = form.UsingNeedCriteria,
                Monetization = form.Monetization,
                EgissoId = form.EgissoId
            };

            foreach (var kpCode in form.Categories.Select(c =>
                new KpCodeLink
                {
                    Id = Guid.NewGuid(),
                    PrivilegeId = privilege.Id,
                    KpCodeId = c.CategoryId,
                    MeasureUnitId = c.MeasureUnitId,
                    Value = c.Value,
                    EgissoId = c.EgissoId,
                    Active = true
                }))
            writeDbContext.Add(kpCode);

            writeDbContext.Add(privilege);
            
        }
    }
}
