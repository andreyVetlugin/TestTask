using AisBenefits.Models.EGISSO.PeriodTypes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Services.EGISSO.PeriodTypes.Edit
{
   
        public class PeriodTypeEditEditFormHandler : IPeriodTypeEditEditFormHandler
        {
            private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
            private readonly IReadDbContext<IBenefitsEntity> readDbContext;

            public PeriodTypeEditEditFormHandler(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext)
            {
                this.writeDbContext = writeDbContext;
                this.readDbContext = readDbContext;
            }

            public OperationResult Handle(PeriodTypeEditForm form, ModelStateDictionary modelState)
            {
                if (!form.Id.HasValue) return OperationResult.BuildFormError("Нет Id ");
                var periodTypeId = form.Id.Value;

            if (!modelState.IsValid)
                    return OperationResult.BuildFormError("Ошибка заполнения формы");

                var periodType = readDbContext.Get<PeriodType>().ById(periodTypeId).FirstOrDefault();
                if (periodType == null)
                    throw new SmtpException(SmtpStatusCode.BadCommandSequence);
                writeDbContext.Attach(periodType);
                periodType.PositionCode = form.PositionCode;
                periodType.PpNumber = form.PpNumber;
                periodType.Value = form.Value;

                writeDbContext.SaveChanges();

                return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
            }
        }
   
}
