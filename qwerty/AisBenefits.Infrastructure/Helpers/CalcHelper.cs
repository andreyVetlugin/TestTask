namespace AisBenefits.Infrastructure.Helpers
{
    public static class CalcHelper
    {
        public static decimal PremiumPerc(decimal premium, decimal salary, decimal uralMultiplier)
        {
            return PremiumMultiplier(premium, salary, uralMultiplier) * 100;
        }
        public static decimal PremiumMultiplier(decimal premium, decimal salary, decimal uralMultiplier) =>
            premium / (salary * uralMultiplier);

        public static decimal MatSupportMultiplier(decimal materialSupport, decimal salary)
        {
            return materialSupport / salary;
        }

        public static decimal VyslugaMultiplier(decimal vysluga, decimal salary, decimal uralMultiplier, decimal defaultValue) =>
            salary != 0 ? VyslugaMultiplier(vysluga, salary, uralMultiplier) : defaultValue;

        public static decimal VyslugaMultiplier(decimal vysluga, decimal salary, decimal uralMultiplier) =>
            vysluga / (salary * uralMultiplier);

        public static decimal SecrecyMultiplier(decimal secrecy, decimal salary, decimal uralMultiplier) =>
            secrecy / (salary * uralMultiplier);

        public static decimal QualificationMultiplier(decimal qualification, decimal salary, decimal uralMultiplier) =>
            qualification / (salary * uralMultiplier);
    }
}
