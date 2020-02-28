using AisBenefits.Infrastructure.Helpers;
using System;

namespace AisBenefits.Infrastructure.Services.ExtraPays
{
    public static class ExtraPayModelHelper
    {
        public static ExtraPayModel BuildModel(this ExtraPayModelData data) =>
            new ExtraPayModel(data);
    }

    public class ExtraPayModel
    {
        private readonly ExtraPayModelData extraPay;

        public ExtraPayModel(ExtraPayModelData extraPay)
        {
            this.extraPay = extraPay;
        }

        public Guid VariantId => extraPay.Variant.Id;

        public int VariantNumber => extraPay.Variant.Number;

        public decimal UralMultiplier => extraPay.Initial ? extraPay.Variant.UralMultiplier ?? 0 : extraPay.Instance.UralMultiplier;

        public decimal Salary => extraPay.Instance.Salary;

        public decimal SalaryMultiplied => Salary * UralMultiplier;

        public decimal Premium => extraPay.Instance.Premium;

        public decimal MaterialSupport => extraPay.Instance.MaterialSupport;

        public decimal Perks => extraPay.Instance.Perks;

        public decimal MatSupportMultiplier => Salary != 0
            ? CalcHelper.MatSupportMultiplier(MaterialSupport, Salary)
            : extraPay.Variant.MatSupportMultiplier ?? 0;

        public decimal MaterialSupportDivPerc => MatSupportMultiplier * 100;

        public decimal PerksDivPerc => UralMultiplier * Salary != 0 
            ? Math.Round(Perks * 100 / (UralMultiplier * Salary))
            : 0;
        
        public decimal VyslugaMultiplier => extraPay.Initial ? extraPay.Variant.VyslugaMultiplier ?? 0 : CalcHelper.VyslugaMultiplier(Vysluga, Salary, UralMultiplier, 0);

        public decimal Vysluga => extraPay.Instance.Vysluga;

        public decimal VyslugaDivPerc => VyslugaMultiplier * 100;
        public decimal Secrecy => extraPay.Instance.Secrecy;
        
        public decimal Qualification => extraPay.Instance.Qualification;
        
        public decimal Ds => extraPay.Instance.Ds;

        public decimal SecrecyDivPerc => SalaryMultiplied != 0
            ? CalcHelper.SecrecyMultiplier(Secrecy, Salary, UralMultiplier) * 100
            : 0;

        public decimal QualificationDivPerc => SalaryMultiplied != 0
            ? CalcHelper.QualificationMultiplier(Qualification, Salary, UralMultiplier) * 100
            : 0;

        public decimal DsPerc => extraPay.Initial ? extraPay.DsPerc.Amount : extraPay.Instance.DsPerc;

        public decimal MinExtraPay => extraPay.MinExtraPay.Value;
        
        public decimal GosPension => extraPay.Instance.GosPension;

        public decimal ExtraPension => extraPay.Instance.ExtraPension;

        public decimal TotalPension => extraPay.Instance.TotalPension;

        public decimal TotalPensionAndExtraPay => extraPay.Instance.TotalPensionAndExtraPay;

        public decimal TotalExtraPay => extraPay.Instance.TotalExtraPay;

        public int WorkAgeDays => extraPay.WorkAgeDays;
        
    }
}
