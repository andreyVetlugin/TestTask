using AisBenefits.Infrastructure.DTOs;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using DataLayer.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AisBenefits.Infrastructure.Services.PersonInfos
{
    public class PersonInfoService : IPersonInfoService
    {
        private const int pageCapacity = 50;

        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly ILogBuilder logBuilder;
        private readonly IDeclarationNumberProvider declarationNumberProvider;

        public PersonInfoService(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext, ILogBuilder logBuilder, IDeclarationNumberProvider declarationNumberProvider)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
            this.logBuilder = logBuilder;
            this.declarationNumberProvider = declarationNumberProvider;
        }

        public PersonInfo Create(IPersonInfoDTO personInfoDto)
        {
            var personInfo = Mapper.Map<IPersonInfoDTO, PersonInfo>(personInfoDto); //Перегоняем из входящего объекта в сущность
            personInfo.Id = Guid.NewGuid();
            personInfo.RootId = personInfo.Id;
            personInfo.CreateTime = DateTime.Now;
            personInfo.CodeEgisso = personInfo.DocTypeId;
            personInfo.Approved = false;
            if (personInfo.Number == 0)
            {
                personInfo.Number = GetNextPersonInfoNumber();
            }
            else
            {
                var check = readDbContext.Get<PersonInfo>().ByNumber(personInfo.Number).FirstOrDefault();
                if (check != null)
                {
                    throw new Exception(message: "Такой номер в базе уже есть увы");
                }
            }

            writeDbContext.Add(personInfo);

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.PersonInfoCREATE, personInfo.RootId); //Логгирование
            writeDbContext.Add(infoLog);

            writeDbContext.SaveChanges();

            return personInfo;
        }
        
        public List<PersonInfo> GetAllPersonInfos()
        {
            var personInfos = readDbContext.Get<PersonInfo>().Active().ToList(); //берём все актуальные версии пользователей

           
            return personInfos;
        }

        public PersonInfo GetPersonInfo(Guid rootId)
        {
            var pers = readDbContext.Get<PersonInfo>().ByRootId(rootId).FirstOrDefault();
            
            return pers;
        }
        
        public void Update(IPersonInfoDTO personInfoDto)
        {
            //var currentUser = currentUserProvider.GetCurrentUser();

            var personInfo = Mapper.Map<IPersonInfoDTO, PersonInfo>(personInfoDto);
            personInfo.Id = Guid.NewGuid();
            personInfo.CreateTime = DateTime.Now;
            personInfo.CodeEgisso = personInfo.DocTypeId;

            if (personInfo.Number == 0)
            {
                personInfo.Number = GetNextPersonInfoNumber();
            }


            var actualPersonInfoVersionInBase = readDbContext.Get<PersonInfo>().ByRootId(personInfo.RootId).FirstOrDefault();

            if (actualPersonInfoVersionInBase == null) throw new Exception("Нет такой карты");

            writeDbContext.Attach(actualPersonInfoVersionInBase);
            actualPersonInfoVersionInBase.NextId = personInfo.Id;
            actualPersonInfoVersionInBase.OutdateTime = DateTime.Now;

            personInfo.Approved = actualPersonInfoVersionInBase.Approved;

            writeDbContext.Add(personInfo);

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.PersonInfoUPDATE, personInfo.RootId);

            writeDbContext.Add(infoLog);

            writeDbContext.SaveChanges();
        }


        public string[] GetDocumentTypes()
        {
            return PersonDocumentTypes.AllTypes;
        }


        public int GetNextPersonInfoNumber()
        {
            return declarationNumberProvider.GenerateNumber();
        }


        public Guid GetActualVersionId(Guid rootId)
        {
            var currentPersonInfo = readDbContext.Get<PersonInfo>().ByRootId(rootId).FirstOrDefault();
            if (currentPersonInfo == null) throw new Exception("нет такой карты");

            return currentPersonInfo.Id;
        }


        public void UpdateByWorkInfo(IConfirmExperienceDto confirmExperienceDto)
        {
            var personInfoId = confirmExperienceDto.PersonInfoId;
            var approved = confirmExperienceDto.Approved;
            var docsSubmitDate = confirmExperienceDto.DocsSubmitDate;
            var docsDestinationDate = confirmExperienceDto.DocsDestinationDate;



            var actualPersonInfoVersionInBase = readDbContext.Get<PersonInfo>().ByRootId(personInfoId).FirstOrDefault();

            if (actualPersonInfoVersionInBase.Approved == approved && actualPersonInfoVersionInBase.DocsSubmitDate == docsSubmitDate && actualPersonInfoVersionInBase.DocsDestinationDate == docsDestinationDate)
                return;
            else
            {
                var newPersonInfo = Mapper.Map<PersonInfo, PersonInfo>(actualPersonInfoVersionInBase);
                newPersonInfo.DocsSubmitDate = docsSubmitDate;
                newPersonInfo.Approved = approved;
                newPersonInfo.Id = Guid.NewGuid();
                newPersonInfo.CreateTime = DateTime.Now;
                newPersonInfo.DocsDestinationDate = docsDestinationDate;

                writeDbContext.Attach(actualPersonInfoVersionInBase);
                actualPersonInfoVersionInBase.NextId = newPersonInfo.Id;
                actualPersonInfoVersionInBase.OutdateTime = DateTime.Now;



                writeDbContext.Add(newPersonInfo);

                var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.PersonInfoUPDATE, newPersonInfo.RootId);


                writeDbContext.Add(infoLog);

                writeDbContext.SaveChanges();

            }
        }

        public void DeactivatePersonInfo(Guid personInfoRootId)
        {
            var actualPersonInfoVersionInBase = readDbContext.Get<PersonInfo>().ByRootId(personInfoRootId).FirstOrDefault();
            if (actualPersonInfoVersionInBase == null) throw new Exception("нет такой карты");

            writeDbContext.Attach(actualPersonInfoVersionInBase);
            actualPersonInfoVersionInBase.OutdateTime = DateTime.Now;
            //actualPersonInfoVersionInBase.NextId = default(Guid);
            actualPersonInfoVersionInBase.StoppedSolutions = true;
            
            writeDbContext.SaveChanges();
        }
        public OperationResult ResumeSolutionForPerson(Guid personInfoRootId)
        {
            var actualPersonInfoVersionInBase = readDbContext.Get<PersonInfo>().ByRootId(personInfoRootId).FirstOrDefault();

            if (actualPersonInfoVersionInBase == null)
                return OperationResult.BuildFormError("Нет такого человека в базе");

            writeDbContext.Attach(actualPersonInfoVersionInBase);
            actualPersonInfoVersionInBase.StoppedSolutions = false;

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.PersonInfoResumeSolution, actualPersonInfoVersionInBase.RootId);
            writeDbContext.Add(infoLog);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }


        public List<PersonInfo> GetArchive(int pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            var personInfosStop = readDbContext.Get<PersonInfo>().AllArchive().ToList();
            var personInfos = personInfosStop.Skip(pageCapacity * (pageNumber - 1)).Take(pageCapacity);
            return personInfos.ToList();
        }

        public int GetPagesCount(int itemsCount)
        {
            return itemsCount % pageCapacity == 0 ? (itemsCount / pageCapacity) : (itemsCount / pageCapacity) + 1;
        }
    }
}
