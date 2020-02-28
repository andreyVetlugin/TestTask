using AisBenefits.Infrastructure.Eip.Pfr.PfrBapForPeriodSmev3;

namespace AisBenefits.Infrastructure.Services.GosPensions
{
    public interface IGosPensionPfrClient: IUnitOfWork
    {
        PfrBapForPeriodClientRequestInitial RequestInitial(IPfrBapForPeriodClientRequestTarget target);
        PfrBapForPeriodClientRequestResult RequestResult(IPfrBapForPeriodClientRequestTarget target);
        void DeleteRequest(IPfrBapForPeriodClientRequestTarget target);
    }
}
