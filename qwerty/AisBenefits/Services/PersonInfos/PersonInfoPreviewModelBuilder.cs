using AisBenefits.Infrastructure.Services.DropDowns;
using AisBenefits.Models.PersonInfos;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System.Linq;

namespace AisBenefits.Services.PersonInfos
{

    public interface IPersonInfoPreviewModelBuilder
    {
        PersonInfoPreviewModel[] Build(IQueryable<PersonInfo> personInfos, int pageNumber, IReadDbContext<IBenefitsEntity> readDbContext, bool searchMode);
    }


    public class PersonInfoPreviewModelBuilder : IPersonInfoPreviewModelBuilder
    {
        public PersonInfoPreviewModel[] Build(IQueryable<PersonInfo> personInfosQuery, int pageNumber, IReadDbContext<IBenefitsEntity> readDbContext, bool searchMode)
        {
            var personInfos =
                (from personInfo in personInfosQuery
                 join extraPay in readDbContext.Get<ExtraPay>().Actual() on personInfo.RootId equals extraPay
                        .PersonRootId into extraPays
                 from extraPay in extraPays.DefaultIfEmpty()
                 join variant in readDbContext.Get<ExtraPayVariant>() on extraPay.VariantId equals variant.Id into
                     variants
                 from variant in variants.DefaultIfEmpty()
                 select new
                 {
                     personInfo,
                     variant
                 })
                .OrderBy(p => searchMode ? p.personInfo.Number : -p.personInfo.Number)
                .Skip((pageNumber - 1) * 50)
                .Take(50)
                .ToList();

            var pausedSolutions = readDbContext.Get<Solution>()
                .LastByPersonInfoRootIds(personInfos.Select(p => p.personInfo.RootId))
                .Paused()
                .Select(s => s.PersonInfoRootId)
                .ToHashSet();


            return personInfos
                .Select(p =>
                    new PersonInfoPreviewModel
                    {
                        RootId = p.personInfo.RootId,
                        SurName = p.personInfo.SurName,
                        Name = p.personInfo.Name,
                        MiddleName = p.personInfo.MiddleName,
                        BirthDate = p.personInfo.BirthDate,
                        Number = p.personInfo.Number,
                        SNILS = p.personInfo.SNILS,
                        EmployType = EmployeeTypes.ShortTitles[p.personInfo.EmployeeTypeId],
                        PayoutType = PayoutTypes.payoutTypes[p.personInfo.PayoutTypeId],
                        Approved = p.personInfo.Approved,
                        Paused = pausedSolutions.Contains(p.personInfo.RootId),
                        ExtraPayVariant = p.variant?.Number.ToString()
                    }).ToArray();
        }
    }
}
