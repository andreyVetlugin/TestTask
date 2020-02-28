using AisBenefits.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Infrastructure.DbContexts;
using DataLayer.Entities;
using AisBenefits.Models.EGISSO.MeasureUnits;
using DataLayer.Entities.EGISSO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net.Mail;

namespace AisBenefits.Services.EGISSO.MeasureUnits
{
    public class MeasureUnitEditEditFormHandler : IMeasureUnitEditEditFormHandler
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public MeasureUnitEditEditFormHandler(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
        }

        public OperationResult Handle(MeasureUnitEditForm form, ModelStateDictionary modelState)
        {
            if (!form.Id.HasValue) return OperationResult.BuildFormError("Нет Id ");
            var measureUnitId = form.Id.Value;

            if (!modelState.IsValid)
                return OperationResult.BuildFormError("Ошибка заполнения формы");

            var measureUnit = readDbContext.Get<MeasureUnit>().FirstOrDefault(c => c.Id == measureUnitId);
            if (measureUnit == null)
                throw new SmtpException(SmtpStatusCode.BadCommandSequence);

            writeDbContext.Attach(measureUnit);
            measureUnit.OkeiCode = form.OkeiCode;
            measureUnit.PositionCode = form.PositionCode;
            measureUnit.PpNumber = form.PpNumber;
            measureUnit.ShortTitle = form.ShortTitle;
            measureUnit.Title = form.Title;
            measureUnit.IsDecimal = form.IsDecimal;
            

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
