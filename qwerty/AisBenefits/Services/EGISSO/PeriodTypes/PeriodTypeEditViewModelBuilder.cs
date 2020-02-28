using AisBenefits.Models.EGISSO.PeriodTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Services.EGISSO.PeriodTypes
{
    public class PeriodTypeEditViewModelBuilder: IPeriodTypeEditViewModelBuilder
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public PeriodTypeEditViewModelBuilder(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
        }

        public PeriodTypeEditViewModel BuildCreate(PeriodTypeEditForm form = null, PeriodTypeEditFormHandlerResult result = null)
        {
            if (form == null)
                form = new PeriodTypeEditForm();

            return new PeriodTypeEditViewModel(form, PeriodTypeEditType.Create, result != null ? result.Error : null);
        }

        public PeriodTypeEditViewModel BuildEdit(Guid periodTypeId, PeriodTypeEditForm form = null, PeriodTypeEditFormHandlerResult result = null)
        {
            var periodType = readDbContext.Get<PeriodType>().ById(periodTypeId).FirstOrDefault();

            if (periodType == null)
                throw new SmtpException(SmtpStatusCode.BadCommandSequence);

            if (form == null)
            {
                form = new PeriodTypeEditForm
                {
                    PositionCode = periodType.PositionCode,
                    PpNumber = periodType.PpNumber,
                    Value = periodType.Value
                };
            }

            return new PeriodTypeEditViewModel(form, PeriodTypeEditType.Edit, result != null ? result.Error : null);
        }
    }
}
