using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AisBenefits.Infrastructure.Services.Egisso
{
    public static class EgissoEjectionHistoryService
    {
        public static IQueryable<EgissoEjectionHistory> GetEgissoEjectionHistories<TContext>(this TContext context)
            where TContext : IReadDbContext<IBenefitsEntity> =>
            context.Get<EgissoEjectionHistory>();

        public static void CreateEgissoEjectionHistory<TContext>(this TContext context, DateTime date, DateTime destinationDate, DateTime from, DateTime to)
            where TContext : IWriteDbContext<IBenefitsEntity> =>
            context.Add(new EgissoEjectionHistory
            {
                Id = Guid.NewGuid(),
                Date = date,
                DestinationDate = destinationDate,
                From = from,
                To = to
            });
    }
}
