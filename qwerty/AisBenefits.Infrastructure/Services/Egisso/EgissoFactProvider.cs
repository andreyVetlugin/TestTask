using AisBenefits.Infrastructure.Services.PersonInfos;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AisBenefits.Infrastructure.Services.Egisso
{
    public static class EgissoFactProvoder
    {
        public static ModelDataResult<IEnumerable<EgissoFactData>> GetEgissoFacts<TContext>(this TContext context, EgissoFactPeriodData period)
            where TContext : IReadDbContext<IBenefitsEntity>
        {
            var personInfoRootIds = context.GetActivePersonInfoRootIds();

            var solutions = context.Get<Solution>()
                .LastByPersonInfoRootIds(personInfoRootIds)
                .ToDictionary(s => s.PersonInfoRootId);

            var personInfos = context.Get<PersonInfo>()
                .ByRootIds(personInfoRootIds)
                .ToList();

            var egissoData =
                (
                from privilege in context.Get<Privilege>().Take(1)
                join categoryLink in context.Get<KpCodeLink>().Active() on privilege.Id equals categoryLink.PrivilegeId
                join provisionForm in context.Get<ProvisionForm>() on privilege.ProvisionFormId equals provisionForm.Id
                join measureUnit in context.Get<MeasureUnit>() on categoryLink.MeasureUnitId equals measureUnit.Id
                select new
                {
                    privilege,
                    categoryLink,
                    provisionForm,
                    measureUnit
                }
                )
                .FirstOrDefault();

            if (egissoData == null)
                return ModelDataResult<IEnumerable<EgissoFactData>>.BuildInnerStateError("Справочники Егиссо не заполнены");

            var ejectionTranform = new EjectionTransform();
            return ModelDataResult<IEnumerable<EgissoFactData>>.BuildSucces(
                personInfos.Select(p => {
                var validationResult = ejectionTranform.ValidateEgissoFact(p);
                    return new EgissoFactData(
                        p,
                        solutions[p.RootId],
                        period,
                        egissoData.privilege,
                        egissoData.categoryLink,
                        egissoData.provisionForm,
                        egissoData.measureUnit,
                        new EgissoFactReciever(
                            new EgissoFactPerson(p),
                            new EgissoFactPersonDocument(p, validationResult.DocumentValid)
                        ),
                        validationResult);
                })
                );
        }
    }
    public class EgissoFactPeriodData
    {
        public DateTime DecisionDate { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public EgissoFactPeriodData(DateTime decisionDate, DateTime startDate, DateTime endDate)
        {
            DecisionDate = decisionDate;
            StartDate = startDate;
            EndDate = endDate;
        }
    }

    public class EgissoFactData : IEgissoFact, IEgissoFactValidationError
    {
        public PersonInfo PersonInfo { get; }
        public Solution Solution { get; }
        public EgissoFactPeriodData PeriodData { get; }

        public Privilege Privilege { get; }
        public KpCodeLink CategoryLink { get; }
        public ProvisionForm ProvisionForm { get; }
        public MeasureUnit MeasureUnit { get; }

        public EgissoFactReciever Reciever { get; }

        public EgissoFactValidationResult ValidationResult { get; }
        public bool Valid => ValidationResult.Ok;

        public EgissoFactData(PersonInfo personInfo, Solution solution, EgissoFactPeriodData periodData, Privilege privilege, KpCodeLink categoryLink, ProvisionForm provisionForm, MeasureUnit measureUnit, EgissoFactReciever reciever, EgissoFactValidationResult validationResult)
        {
            PersonInfo = personInfo;
            Solution = solution;
            PeriodData = periodData;
            Privilege = privilege;
            CategoryLink = categoryLink;
            ProvisionForm = provisionForm;
            MeasureUnit = measureUnit;
            Reciever = reciever;
            ValidationResult = validationResult;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public Guid PrivilegeId => Privilege.EgissoId;

        public Guid CategoryId => CategoryLink.EgissoId;

        public bool UsingNeedCriteria => Privilege.UsingNeedCriteria;

        public DateTime DecisionDate => PeriodData.DecisionDate;

        public DateTime StartDate => PeriodData.StartDate;

        public DateTime EndDate => PeriodData.EndDate;

        IEgissoFactReciever IEgissoFact.Reciever => Reciever;

        IEgissoFactReason IEgissoFact.Reason => null;

        public string ProvisionFormCode => ProvisionForm.Code;

        public decimal Amount => Solution.TotalExtraPay;

        public bool IsDecimal => MeasureUnit.IsDecimal;

        public string MeasureCode => MeasureUnit.PositionCode;

        public bool Monetization => Privilege.Monetization;

        public Guid RecieverId => PersonInfo.RootId;

        public int RecieverNumber => PersonInfo.Number;

        IReadOnlyList<string> IEgissoFactValidationError.Errors => ValidationResult.Errors;
    }
    public class EgissoFactReciever : IEgissoFactReciever
    {
        public EgissoFactPerson Person { get; }
        public EgissoFactPersonDocument Document { get; }

        public EgissoFactReciever(EgissoFactPerson person, EgissoFactPersonDocument document)
        {
            Person = person;
            Document = document;
        }

        IEgissoFactPerson IEgissoFactReciever.Person => Person;

        IEgissoFactPersonDocument IEgissoFactReciever.Document => Document;
    }

    public class EgissoFactPerson : IEgissoFactPerson
    {
        public PersonInfo PersonInfo { get; }

        public EgissoFactPerson(PersonInfo personInfo)
        {
            PersonInfo = personInfo;
        }

        public string LastName => PersonInfo.SurName;

        public string FirstName => PersonInfo.Name;

        public string SurName => PersonInfo.MiddleName;

        public char Gender => PersonInfo.Sex;

        public DateTime BirthDate => PersonInfo.BirthDate;

        public string Snils => PersonInfo.SNILS;
    }
    public class EgissoFactPersonDocument : IEgissoFactPersonDocument
    {
        public PersonInfo PersonInfo { get; }

        public EgissoFactPersonDocument(PersonInfo personInfo, bool valid)
        {
            PersonInfo = personInfo;
            Valid = valid;
        }

        public string Type => PersonInfo.DocTypeId;

        public string Seria => PersonInfo.DocSeria;

        public string Number => PersonInfo.DocNumber;

        public DateTime IssueDate => PersonInfo.IssueDate.Value;

        public string IssuePlace => PersonInfo.Issuer;

        public bool Valid { get; }
    }

}
