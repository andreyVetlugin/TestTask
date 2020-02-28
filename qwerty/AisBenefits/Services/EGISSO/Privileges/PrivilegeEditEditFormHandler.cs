using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.Privileges;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.Privileges
{
    public class PrivilegeEditEditFormHandler : IPrivilegeEditEditFormHandler
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly ICurrentUserProvider currentUserProvider;

        public PrivilegeEditEditFormHandler(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext, ICurrentUserProvider currentUserProvider)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
            this.currentUserProvider = currentUserProvider;
        }

        public OperationResult Handle(PrivilegeEditForm form, ModelStateDictionary modelState)
        {
            if (!form.Id.HasValue) return OperationResult.BuildFormError("Нет Id ");
            var privilegeId = form.Id.Value;

            var editResult = EditPrivilege(privilegeId, form);

            return editResult;
        }

        private OperationResult EditPrivilege(Guid privilegeId, PrivilegeEditForm form)
        {
            var privilege = readDbContext.Get<Privilege>().FirstOrDefault(c => c.Id == privilegeId);

            if (privilege == null)
                return OperationResult.BuildFormError("Попытка редактирования несуществующей меры социальной поддержки");

            writeDbContext.Attach(privilege);

            privilege.Title = form.Title;
            privilege.EgissoCode = form.EgissoCode;
            privilege.PeriodTypeId = form.PeriodTypeId;
            privilege.ProvisionFormId = form.ProvisionFormId;
            privilege.UsingNeedCriteria = form.UsingNeedCriteria;
            privilege.Monetization = form.Monetization;
            
                privilege.EgissoId = form.EgissoId;

            var privilegeCategories = readDbContext.Get<KpCodeLink>()
                                                          .Where(p=>p.Active&&p.PrivilegeId==privilegeId)                                                          
                                                          .ToDictionary(c => c.KpCodeId);

            //List<Guid> needUpdateList = new List<Guid>();
            //var addedCategories = new List<KpCodeLink>();
            foreach (var category in form.Categories)
            {
                var currentCategory = privilegeCategories.GetValueOrDefault(category.CategoryId);

                var createNew = currentCategory == null;
                if (!createNew && category.HasChanges(currentCategory.MeasureUnitId, currentCategory.Value,
                        currentCategory.EgissoId))
                {
                    writeDbContext.Attach(currentCategory);
                    currentCategory.Active = false;
                    createNew = true;
                }

                if (createNew)
                {
                    var newCategory = new KpCodeLink
                    {
                        Id = Guid.NewGuid(),
                        PrivilegeId = privilegeId,
                        KpCodeId = category.CategoryId,
                        MeasureUnitId = category.MeasureUnitId,
                        Value = category.Value,
                        EgissoId = category.EgissoId,
                        Active = true
                    };
                    writeDbContext.Add(newCategory);

                }
            }

            foreach (var kpCodeLink in privilegeCategories.Values.Where(c => form.Categories.All(f => f.CategoryId != c.KpCodeId)))
            {
                writeDbContext.Attach(kpCodeLink);
                kpCodeLink.Active = false;
            }


            //foreach (var categoryId in needUpdateList)
            //    UpdatePersonPrivilegeLinks(privilegeId, categoryId);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        
    }
}
