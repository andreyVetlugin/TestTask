using AisBenefits.Models.ExcelReport;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Services.Excel
{
    public interface IExcelReportFilter
    {
        IEnumerable<Guid> GetPersonsRootIds(ExcelReportForm form, bool archive=false);
    }

    public class ExcelReportFilter : IExcelReportFilter
    {

        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public ExcelReportFilter(IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.readDbContext = readDbContext;
        }



        public IEnumerable<Guid> GetPersonsRootIds(ExcelReportForm form, bool archive = false)
        {
            var t = PersonInfoFilter(form.PIForm, archive).ToHashSet();

            if(WorkInfoFilter(form.WIForm, out var wiFilter))
                t.IntersectWith(wiFilter);
            if(ExtraPayFilter(form.EPForm, out var extraPayFilter))
                t.IntersectWith(extraPayFilter);
           if(SolutionFilter(form.SolForm, archive, out var solutionFilter))
                t.IntersectWith(solutionFilter);
            
            return t;

        }


        private IEnumerable<Guid> PersonInfoFilter(PersonInfoReportForm form, bool archive = false)
        {
            var list = archive ? readDbContext.Get<PersonInfo>().AllArchive()
                :readDbContext.Get<PersonInfo>().Active();

            var res = list
                .FilterBy(c => c.AdditionalPensionId, form.AdditionalPensionId)
                .FilterBy(c => c.DistrictId, form.DistrictId)
                .FilterBy(c => c.EmployeeTypeId, form.EmployeeTypeId)
                .FilterBy(c => c.PayoutTypeId, form.PayoutTypeId)
                .FilterBy(c => c.PensionTypeId, form.PensionTypeId)
                .FilterBy(c => c.Approved, form.Approved)

                .Select(c => c.RootId)
                .ToList();

            return res;

        }

        private bool WorkInfoFilter(WorkInfoReportForm form, out IEnumerable<Guid> filter)
        {
            if (!(form.FunctionId.IsFiltered ||
                  form.OrganizationId.IsFiltered))
            {
                filter = null;
                return false;
            }

            var list = readDbContext.Get<WorkInfo>().AllActual();

            filter = list
                .FilterBy(c => c.FunctionId, form.FunctionId)
                .FilterBy(c => c.OrganizationId, form.OrganizationId)
                .Select(c => c.PersonInfoRootId)
                .ToHashSet();
            return true;
        }

        private bool ExtraPayFilter(ExtraPayReportForm form, out IEnumerable<Guid> filter)
        {
            if (!(form.VariantId.IsFiltered ||
                  form.MaterialSupportDivPerc.IsFiltered ||
                  form.PerksDivPerc.IsFiltered ||
                  form.Premium.IsFiltered ||
                  form.QualificationDivPerc.IsFiltered ||
                  form.SecrecyDivPerc.IsFiltered ||
                  form.VariantId.IsFiltered ||
                  form.VyslugaDivPerc.IsFiltered))
            {
                filter = null;
                return false;
            }

            var list = readDbContext.Get<ExtraPay>().Actual();

            filter = list
                .FilterBy(c => CountDivPerc(c.MaterialSupport, c.Salary, c.UralMultiplier), form.MaterialSupportDivPerc)
                .FilterBy(c => CountDivPerc(c.Perks, c.Salary, c.UralMultiplier), form.PerksDivPerc)
                .FilterBy(c => CountDivPerc(c.Premium, c.Salary, c.UralMultiplier), form.Premium)
                .FilterBy(c => CountDivPerc(c.Qualification, c.Salary, c.UralMultiplier), form.QualificationDivPerc)
                .FilterBy(c => CountDivPerc(c.Secrecy, c.Salary, c.UralMultiplier), form.SecrecyDivPerc)
                .FilterBy(c => c.VariantId, form.VariantId)
                .FilterBy(c => CountDivPerc(c.Vysluga, c.Salary, c.UralMultiplier), form.VyslugaDivPerc)
                .Select(c => c.PersonRootId)
                .ToList();
            return true;
        }

        private decimal CountDivPerc(decimal value, decimal salary, decimal uMul)
        {
            var salaryMultip = salary * uMul;
            return salaryMultip == 0 ? 0 : Math.Round((value * 100) / salaryMultip);
        }


        private bool SolutionFilter(SolutionReportForm form, bool archive, out IEnumerable<Guid> filter)
        {
            if (!(form.Type.IsFiltered || archive) )
            {
                filter = null;
                return false;
            }

            var list = (archive
                ? readDbContext.Get<Solution>().Archive()
                : readDbContext.Get<Solution>().Positive());

            if (archive)
            {
                filter = list.Select(c => c.PersonInfoRootId).ToList();
                return true;
            }

            filter = list
                .FilterBy(c => c.Type, form.Type)
                .Select(c => c.PersonInfoRootId)
                .ToList();
            return true;
        }

    }
}
