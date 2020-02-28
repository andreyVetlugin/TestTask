using AisBenefits.Models.ExcelReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using AisBenefits.Services.Word;

namespace AisBenefits.Services.Excel
{

    public class ExcelReportFilterSelectorBuilder
    {

        public static List<TableItemSelector<ExcelSourceModel>> Build(ExcelReportForm form)
        {
            var selectors = new List<TableItemSelector<ExcelSourceModel>>();
            var pf = form.PIForm;
            var wf = form.WIForm;
            var ep = form.EPForm;
            var sol = form.SolForm;
            var po = form.PayoutForm;


            if (pf.Number.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Номер", m =>
                    m.PersonInfo.Number));
            if (pf.Fio.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("ФИО", m =>
                    $"{m.PersonInfo.SurName} {m.PersonInfo.Name} {m.PersonInfo.MiddleName}"));
            if (pf.Sex.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Пол", m =>
                    m.PersonInfo.Sex=='М'? "Мужской" : "Женский"));
            if (pf.BirthDate.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Дата рождения", m =>
                    m.PersonInfo.BirthDate.ToShortDateString()));
            if (pf.Birthplace.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Место рождения", m =>
                    m.PersonInfo.Birthplace));
            if (pf.SNILS.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("СНИЛС", m =>
                    m.PersonInfo.SNILS));
            if (pf.DocSeria.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Документ: Серия", m =>
                    m.PersonInfo.DocSeria));
            if (pf.DocNumber.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Документ: Номер", m =>
                    m.PersonInfo.DocNumber));
            if (pf.Issuer.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Кем выдан", m =>
                    m.PersonInfo.Issuer));
            if (pf.IssueDate.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Когда выдан",
                    m => { return m.PersonInfo.IssueDate != null ? m.PersonInfo.IssueDate.Value.ToShortDateString() : string.Empty; }));
                   
            if (pf.Address.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Адрес", m =>
                    m.PersonInfo.Address));
            if (pf.Phone.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Телефон", m =>
                    m.PersonInfo.Phone));
            if (pf.EmployeeTypeId.IsShow)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Тип получателя", m =>
                    m.PersonInfo.EmployeeType));
            }
            if (pf.PayoutTypeId.IsShow)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Тип выплат", m =>
                    m.PersonInfo.PayoutType));
            }
            if (pf.DistrictId.IsShow)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Район получения пенсии", m =>
                    m.PersonInfo.District));
            }
            if (pf.PensionCaseNumber.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Номер пенсионного дела", m =>
                    m.PersonInfo.PensionCaseNumber));
            if (pf.PensionTypeId.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Тип пенсии", m =>
                    m.PersonInfo.PensionType));
            if (pf.PensionEndDate.IsShow)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Срок выплаты по..", m =>
                    {
                        if (m.PersonInfo.PensionEndDate != null)
                            return m.PersonInfo.PensionEndDate.Value.ToShortDateString();
                        else return string.Empty;
                    }
                    ));
            }
                
            if (pf.AdditionalPensionId.IsShow)
            {
                //var text = ReadDbContext<
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Дополнительная пенсия", m =>
                    m.PersonInfo.AdditionalPension));
            }


            // End PERSONINFO data PersonInfoWordModelBuilder
            if (wf.Experience.IsShow)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Общий стаж", m => 
                    (PersonInfoWordModelBuilder.FormatExp(m.WorkInfo.AgeYears,m.WorkInfo.AgeMonths, m.WorkInfo.AgeDays))
                    ));
            }

            if (wf.DismissalDate.IsShow)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Дата увольнения", m =>
                    {
                        return m.WorkInfo.WorkPlaces.Length > 0
                            ? m.WorkInfo.WorkPlaces.Select(c => c.EndDate).Max().ToShortDateString()
                            : string.Empty;
                    }
                    ));
            }
            if (pf.Approved.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Стаж подтверждён",
                    m => { return m.WorkInfo.Approved ? "Да" : "Нет"; }));
            if (wf.DocsSubmitDate.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Дата подачи документов",
                    m => { return m.WorkInfo.DocsSubmitDate != null ? m.WorkInfo.DocsSubmitDate.Value.ToShortDateString() : string.Empty; }));
            if (wf.DocsDestinationDate.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Дата назначения доплаты",
                    m => { return m.WorkInfo.DocsDestinationDate != null ? m.WorkInfo.DocsDestinationDate.Value.ToShortDateString() : string.Empty; }));

            if (wf.OrganizationId.IsShow)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Организация", m =>
                    {
                        if (m.WorkInfo.WorkPlaces.Length < 1) return string.Empty;
                    if (wf.OrganizationId.IsFiltered)
                    {
                        return m.WorkInfo.WorkPlaces.OrderByDescending(c=>c.StartDate).FirstOrDefault(c => c.OrganizationId == wf.OrganizationId.Value)
                            .OrganizationName;
                    }
                    else
                    {
                        return m.WorkInfo.WorkPlaces.OrderByDescending(c => c.StartDate).FirstOrDefault().OrganizationName;
                    }
                }
                    ));
            }

            if (wf.FunctionId.IsShow)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Должность", m =>
                {
                    if (m.WorkInfo.WorkPlaces.Length < 1) return string.Empty;
                    if (wf.FunctionId.IsFiltered)
                    {
                        return m.WorkInfo.WorkPlaces.OrderByDescending(c => c.StartDate).FirstOrDefault(c => c.FunctionId == wf.FunctionId.Value)
                            .Function;
                    }
                    else
                    {
                        return m.WorkInfo.WorkPlaces.OrderByDescending(c => c.StartDate).FirstOrDefault().Function;
                    }
                }));
            }
            if (wf.StartDate.IsShow&& wf.OrganizationId.IsFiltered&& wf.FunctionId.IsFiltered)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("С", m =>
                    m.WorkInfo.WorkPlaces.FirstOrDefault(c => c.FunctionId == wf.FunctionId.Value).StartDate.ToShortDateString()));
            }
            if (wf.EndDate.IsShow && wf.OrganizationId.IsFiltered && wf.FunctionId.IsFiltered)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("По", m =>
                    m.WorkInfo.WorkPlaces.FirstOrDefault(c => c.FunctionId == wf.FunctionId.Value
                                                              && c.OrganizationId == wf.OrganizationId.Value).EndDate.ToShortDateString()));
            }

            ////End WORKINFO data
            if (ep.VariantId.IsShow)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Вариант расчёта", 
                    m => m.ExtraPayVariant==null? "Нет"
                    : m.ExtraPayVariant.Number.ToString())); ;
            }

            if (ep.UralMultiplier.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Уральский коэффициент", m =>
                    Math.Round(m.ExtraPay.UralMultiplier, 2)));
            if (ep.Salary.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Оклад", m =>
                    Math.Round(m.ExtraPay.Salary, 2)));
            if (ep.SalaryMultiplied.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>(" Оклад с уральским коэффициентом", m =>
                    Math.Round(m.ExtraPay.SalaryMultiplied, 2)));
            if (ep.Premium.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Премия", m =>
                    Math.Round(m.ExtraPay.Premium, 2)));
            if (ep.MaterialSupport.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Матпомощь", m =>
                    Math.Round(m.ExtraPay.MaterialSupport, 2)));
            if (ep.MaterialSupportDivPerc.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Матпомощь %", m =>
                   Math.Round(m.ExtraPay.MaterialSupportDivPerc,2)));
            if (ep.Perks.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Надбавка", m =>
                    Math.Round(m.ExtraPay.Perks, 2)));
            if (ep.PerksDivPerc.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Надбавка %", m =>
                    Math.Round(m.ExtraPay.PerksDivPerc, 2)));
            if (ep.Vysluga.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Выслуга", m =>
                    Math.Round(m.ExtraPay.Vysluga, 2)));
            if (ep.VyslugaDivPerc.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Выслуга %", m =>
                    Math.Round(m.ExtraPay.VyslugaDivPerc, 2)));
            if (ep.Secrecy.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Секретность", m =>
                    Math.Round(m.ExtraPay.Secrecy, 2)));
            if (ep.SecrecyDivPerc.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Секретность %", m =>
                    Math.Round(m.ExtraPay.SecrecyDivPerc, 2)));
            if (ep.Qualification.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Классный чин", m =>
                    Math.Round(m.ExtraPay.Qualification, 2)));
            if (ep.QualificationDivPerc.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Классный чин %", m =>
                    Math.Round(m.ExtraPay.QualificationDivPerc, 2)));
            if (ep.Ds.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Денежное содержание", m =>
                    Math.Round(m.ExtraPay.Ds, 2)));
            if (ep.DsPerc.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Денежное содержание %", m =>
                    Math.Round(m.ExtraPay.DsPerc, 2)));
            if (ep.GosPension.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Госпенсия", m =>
                    Math.Round(m.ExtraPay.GosPension, 2)));
            if (ep.ExtraPension.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Дополнительная пенсия", m =>
                    Math.Round(m.ExtraPay.ExtraPension, 2)));
            if (ep.TotalPension.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Основная пенсия (итог)", m =>
                    Math.Round(m.ExtraPay.TotalPension, 2)));
            if (ep.TotalPensionAndExtraPay.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("МДС (итог)", m =>
                    Math.Round(m.ExtraPay.TotalPensionAndExtraPay, 2)));
            if (ep.TotalExtraPay.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Доплата (итог)", m =>
                    Math.Round(m.ExtraPay.TotalExtraPay, 2)));

            // END EXTRAPay data

            if (sol.Type.IsShow)
            {
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Тип решения", m =>
                    m.Solution != null ?
                    Solution.ConvertTypeToString(m.Solution.Type) :
                    string.Empty));
            }

            if (sol.Destination.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Дата назначения", m =>
                    m.Solution != null ?
                    m.Solution.Destination.ToShortDateString() :
                    string.Empty));
            if (sol.Execution.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Дата исполнения", m =>
                    m.Solution != null ?
                    m.Solution.Execution.ToShortDateString() :
                    string.Empty));

            //END SOLUTION TYPE

            if (po.LastPayDate.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Дата последнего платежа", m =>
                    m.LastPaymentDate==default(DateTime)
                    ? "Нет"
                    :m.LastPaymentDate.ToShortDateString()));
            if (po.LastPaySumm.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Сумма последнего платежа", m =>
                    m.LastReestrElement.Summ));
            if (po.BankCardType.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Тип счёта", 
                    m => m.PersonBankCard==null ?
                        "Нет"
                        : m.PersonBankCard.Type == 0 ? "Карта" : "Счёт"));
            if (po.BankCardNumber.IsShow)
                selectors.Add(new TableItemSelector<ExcelSourceModel>("Номер счёта", 
                    m => m.PersonBankCard == null ?
                        "Нет"
                    :m.PersonBankCard.Number));


            return selectors;

        }
    }
}
