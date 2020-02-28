using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.MeasureUnits;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AisBenefits.Services.EGISSO.MeasureUnits
{
    public class MeasureUnitEditCreateFormHandler : IMeasureUnitEditCreateFormHandler
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;

        public MeasureUnitEditCreateFormHandler(IWriteDbContext<IBenefitsEntity> writeDbContext)
        {
            this.writeDbContext = writeDbContext;
        }

        public OperationResult Handle(MeasureUnitEditForm form, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                return OperationResult.BuildFormError("Ошибка заполнения формы");

            var measureUnit = new MeasureUnit
            {
                Id = Guid.NewGuid(),
                OkeiCode = form.OkeiCode,
                PositionCode = form.PositionCode,
                PpNumber = form.PpNumber,
                ShortTitle = form.ShortTitle,
                Title = form.Title,
                IsDecimal = form.IsDecimal
            };

            writeDbContext.Add(measureUnit);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
