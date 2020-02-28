using System;
using System.Linq;
using AisBenefits.Infrastructure.DTOs;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.PersonInfos
{
    public class PersonBankCardService : IPersonBankCardService
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly ILogBuilder logBuilder;

        public PersonBankCardService(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext, ILogBuilder logBuilder)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
            this.logBuilder = logBuilder;
        }

        public OperationResult Create(IPersonBankCardDto personBankCardDTO)
        {
            var personInfoCheckCount = readDbContext.Get<PersonInfo>().ByRootId(personBankCardDTO.PersonRootId).Count();

            if (personInfoCheckCount < 1)
                return OperationResult.BuildFormError("нет такого пользователя в системе");
                

            var currentCard = readDbContext.Get<PersonBankCard>().ActualByPersonRootId(personBankCardDTO.PersonRootId).FirstOrDefault();

            if (currentCard!=null)
            {
                return OperationResult.BuildFormError("Нужен другой метод, карта у этого пользователя уже есть");
            }

            var personBankCard = Mapper.Map<IPersonBankCardDto, PersonBankCard>(personBankCardDTO);
            personBankCard.CreateDate = DateTime.Now;
            personBankCard.Id = Guid.NewGuid();

            writeDbContext.Add(personBankCard);


            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.PersonBankCardCreate, personBankCard.Id); //Логгирование
            writeDbContext.Add(infoLog);
            
            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }

        public ModelDataResult<PersonBankCard> Get(Guid personInfoRootId)
        {
            var card = readDbContext.Get<PersonBankCard>().ActualByPersonRootId(personInfoRootId).FirstOrDefault()
                ??
                new PersonBankCard
                {
                    PersonRootId = personInfoRootId
                };

            return ModelDataResult<PersonBankCard>.BuildSucces(card);
        }

        public OperationResult Update(IPersonBankCardDto personBankCardDTO)
        {
            var currentCard = readDbContext.Get<PersonBankCard>().ActualByPersonRootId(personBankCardDTO.PersonRootId).FirstOrDefault();
            if (currentCard == null)
            {
                return OperationResult.BuildFormError("Нет такой карты увы");
            }

            var newPersonBankCard = Mapper.Map<IPersonBankCardDto, PersonBankCard>(personBankCardDTO);
            newPersonBankCard.CreateDate = DateTime.Now;
            newPersonBankCard.Id = Guid.NewGuid();

           
            writeDbContext.Attach(currentCard);
            currentCard.OutDate = DateTime.Now;

            writeDbContext.Add(newPersonBankCard);


            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.PersonBankCardUpdate, newPersonBankCard.Id); //Логгирование
            writeDbContext.Add(infoLog);
            
            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
