using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Infrastructure.Services.ExtraPays;
using AisBenefits.Models.ExcelReport;
using AisBenefits.Services.PersonInfos;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Services.Excel
{
    public interface IExcelReportObjectDataBuilder
    {
        ModelDataResult<IEnumerable<ExcelSourceModel>> Build(IEnumerable<Guid> personRootIds);
    }

    public class ExcelReportObjectDataBuilder : IExcelReportObjectDataBuilder
    {
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IPersonInfoModelBuilder personInfoModelBuilder;
        private readonly IWorkInfoModelBuilder workInfoModelBuilder;

        public ExcelReportObjectDataBuilder(IReadDbContext<IBenefitsEntity> readDbContext,
                                            IPersonInfoModelBuilder personInfoModelBuilder,
                                            IWorkInfoModelBuilder workInfoModelBuilder)
        {
            this.readDbContext = readDbContext;
            this.personInfoModelBuilder = personInfoModelBuilder;
            this.workInfoModelBuilder = workInfoModelBuilder;
        }


        public ModelDataResult<IEnumerable<ExcelSourceModel>> Build(IEnumerable<Guid> personRootIds)
        {
            var result = new List<ExcelSourceModel>();
            foreach (var personInfoRootId in personRootIds)
            {
                var personInfo = readDbContext.Get<PersonInfo>()
                    .ByRootId(personInfoRootId).FirstOrDefault();
                var personInfoModel = personInfoModelBuilder.Build(personInfo);
                var workInfoResult = workInfoModelBuilder.Build(personInfo);
                if(!workInfoResult.Ok)
                    return ModelDataResult<IEnumerable<ExcelSourceModel>>.BuildErrorFrom(workInfoResult);
                var workInfo = workInfoResult.Data;
               // var extraPay = readDbContext.Get<ExtraPay>().ActualByPersonRootId(personInfoRootId).FirstOrDefault();
               var extraPayModelDataResult = readDbContext.GetExtraPayModelData(personInfoRootId);
               if (!extraPayModelDataResult.Ok) return ModelDataResult<IEnumerable<ExcelSourceModel>>.BuildErrorFrom(extraPayModelDataResult);
               var extraPay = extraPayModelDataResult.Data.BuildModel();

                 var solution = readDbContext.Get<Solution>().Actual().ByPersonRootId(personInfoRootId).FirstOrDefault();
                var personBankCard = readDbContext.Get<PersonBankCard>().ActualByPersonRootId(personInfoRootId).FirstOrDefault();
                var extraPayVariant = readDbContext.Get<ExtraPayVariant>().ById(extraPay.VariantId).FirstOrDefault();

                var reestrElement = readDbContext.Get<ReestrElement>().ByPersonInfoRootId(personInfoRootId)
                    .OrderByDescending(c => c.To).FirstOrDefault() ?? new ReestrElement();

                DateTime lastPaymentDate = default(DateTime);

                if (reestrElement.Id !=default(Guid))
                {
                    var reestr = readDbContext.Get<Reestr>().ByReestrId(reestrElement.ReestrId).FirstOrDefault();
                    lastPaymentDate = reestr.Date;
                }

                var dataObject = new ExcelSourceModel
                {
                    PersonInfo = personInfoModel,
                    WorkInfo = workInfo,
                    ExtraPay = extraPay,
                    Solution = solution,
                    PersonBankCard = personBankCard,
                    ExtraPayVariant = extraPayVariant,
                    LastReestrElement = reestrElement,
                    LastPaymentDate = lastPaymentDate

                };
                result.Add(dataObject);
            }


            return ModelDataResult<IEnumerable<ExcelSourceModel>>.BuildSucces(result.OrderBy(c => c.PersonInfo.Number));
        }
    }
}
