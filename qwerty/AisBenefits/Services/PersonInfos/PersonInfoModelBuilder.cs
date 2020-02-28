using AisBenefits.Infrastructure.DTOs;
using AisBenefits.Infrastructure.Services.DropDowns;
using AisBenefits.Models.PersonInfos;
using AutoMapper;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Services.PersonInfos
{

    public interface IPersonInfoModelBuilder
    {
        PersonInfoModel Build(PersonInfo personInfo);
        PersonInfoModel[] Build(List<PersonInfo> personInfo);

    }


    public class PersonInfoModelBuilder : IPersonInfoModelBuilder
    {

        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public PersonInfoModelBuilder(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }

        public PersonInfoModel Build(PersonInfo personInfo)
        {

            var personInfoModel = Mapper.Map<PersonInfo, PersonInfoModel>(personInfo);
            var personInfoRootCreateTime = readDbContext.Get<PersonInfo>().ById(personInfo.RootId).Select(c => c.CreateTime).FirstOrDefault();

            personInfoModel.AdditionalPension = AdditionalPensionTypes.additionalPensionTypes.GetValueOrDefault(personInfo.AdditionalPensionId);
            personInfoModel.DocType = DocumentTypes.documentTypes.GetValueOrDefault(personInfo.DocTypeId);
            personInfoModel.EmployeeType = EmployeeTypes.employeeTypes.GetValueOrDefault(personInfo.EmployeeTypeId);
            personInfoModel.PensionType = PensionTypes.pensionTypes.GetValueOrDefault(personInfo.PensionTypeId);
            personInfoModel.PayoutType = PayoutTypes.payoutTypes.GetValueOrDefault(personInfo.PayoutTypeId);
            personInfoModel.District = Districts.districts.GetValueOrDefault(personInfo.DistrictId);
            personInfoModel.RegistrationDate = personInfoRootCreateTime;




            return personInfoModel;
        }


        public PersonInfoModel[] Build(List<PersonInfo> personInfoList)
        {
            var result = new List<PersonInfoModel>();
            foreach (var personInfo in personInfoList)
            {
                result.Add(Build(personInfo));
            }

            return result.ToArray();
        }

    }
}
