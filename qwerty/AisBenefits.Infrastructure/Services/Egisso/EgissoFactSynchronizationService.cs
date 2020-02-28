using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;

namespace AisBenefits.Infrastructure.Services.Egisso
{
    public static class EgissoFactSynchronizationService
    {
        public static void CreateEgissoFacts<TContext>(this TContext context, IEnumerable<EgissoFactData> facts)
            where TContext: IWriteDbContext<IBenefitsEntity>
        {
            foreach (var fact in facts)
                context.Add(new EgissoFact
                {
                    Id = Guid.NewGuid(),

                    PersonInfoId = fact.PersonInfo.Id,
                    PersonInfoRootId = fact.PersonInfo.RootId,

                    SolutionId = fact.Solution.Id,

                    Date = DateTime.Now,
                    DecisionDate = fact.DecisionDate,
                    StartDate = fact.StartDate,
                    EndDate = fact.EndDate,
                    
                    PrivilegeId = fact.PrivilegeId,
                    CategoryLinkId = fact.CategoryLink.Id,
                    ProvisionFormId = fact.ProvisionForm.Id,
                    MeasureUnitId = fact.MeasureUnit.Id
                });
        }
    }
}
