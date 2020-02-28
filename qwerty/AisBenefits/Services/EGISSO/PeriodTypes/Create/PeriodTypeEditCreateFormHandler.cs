using AisBenefits.Models.EGISSO.PeriodTypes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Services.EGISSO.PeriodTypes.Create
{
    public class PeriodTypeEditCreateFormHandler : IPeriodTypeEditCreateFormHandler
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        public PeriodTypeEditCreateFormHandler(IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.writeDbContext = writeDbContext;
        }

        public OperationResult Handle(PeriodTypeEditForm form, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                return OperationResult.BuildFormError("Ошибка заполнения формы");

            var measureUnit = new PeriodType
            {
                Id = Guid.NewGuid(),
                PositionCode = form.PositionCode,
                PpNumber = form.PpNumber,
                Value = form.Value
            };

            writeDbContext.Add(measureUnit);

            writeDbContext.SaveChanges();

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
