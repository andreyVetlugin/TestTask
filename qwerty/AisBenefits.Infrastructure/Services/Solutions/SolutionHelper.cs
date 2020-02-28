using System;
using System.Collections.Generic;
using System.Linq;
using AisBenefits.Infrastructure.Helpers;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Solutions
{
    public static class SolutionHelper
    {
        public static ModelDataResult<SolutionModelData> GetSolutionModelData(this IReadDbContext<IBenefitsEntity> readDbContext, ExtraPay extraPay)
        {
            var solution = readDbContext.Get<Solution>()
                .TheLastByPersonRootId(extraPay.PersonRootId)
                .FirstOrDefault();

            var data = new SolutionModelData(solution, extraPay);

            return ModelDataResult<SolutionModelData>.BuildSucces(data);
        }

        public static ModelDataResult<List<SolutionModelData>> GetSolutionsModelData(
            this IReadDbContext<IBenefitsEntity> readDbContext, IReadOnlyList<ExtraPay> extraPays)
        {
            var solutions = readDbContext.Get<Solution>()
                .LastByPersonInfoRootIds(extraPays.Select(p => p.PersonRootId))
                .ToDictionary(s => s.PersonInfoRootId);

            var data = extraPays
                .Select(p => new SolutionModelData(solutions.GetValueOrDefault(p.PersonRootId), p))
                .ToList();

            return ModelDataResult<List<SolutionModelData>>.BuildSucces(data);
        }

        public static decimal CalculateSummForMonth(IEnumerable<Solution> solutions, DateTime dateMonth)
        {
            var currentMonthDays = DateHelper.GetNumberDaysInMonth(dateMonth);
            var thisMonthSolutions =
                solutions.Where(c => c.Execution.Month == dateMonth.Month && c.Execution.Year == dateMonth.Year)
                    .OrderByDescending(c=>c.Execution)
                    .ToList();

            var thismonth = new DateTime(dateMonth.Year, dateMonth.Month, 1);
            var oldSolution = solutions
                .FirstOrDefault(c =>
                    (thisMonthSolutions.FirstOrDefault() == null || c.Execution < thisMonthSolutions.FirstOrDefault().Execution) &&
                    c.Execution < thismonth);

            decimal sum = 0;
            int daysModifer = 1;
            if (thisMonthSolutions.Count > 0)
            {
                var days = currentMonthDays;
                foreach (var sol in thisMonthSolutions)
                {
                    if (sol.Type == SolutionType.Pause)
                    {
                        days = sol.Execution.Day - 1;
                        continue;
                    }
                    var payDays = (days - sol.Execution.Day + daysModifer);
                    //daysModifer = 0;
                    var solSum = (Convert.ToDecimal(payDays) / currentMonthDays) * sol.TotalExtraPay;
                    sum += solSum;
                    days = sol.Execution.Day - 1;
                }

                if (oldSolution != null && oldSolution.IsPositive())
                {
                    var ostSum = (Convert.ToDecimal(days) / currentMonthDays) * oldSolution.TotalExtraPay;
                    sum += ostSum;
                }

            }
            else if(oldSolution.IsPositive())
            {
                sum = oldSolution.TotalExtraPay;
            }

            return sum;
        }


    }

    public class SolutionModelData
    {
        public Solution Instance { get; }
        public bool Defined => Instance != null;

        public ExtraPay ExtraPay { get; }

        public SolutionModelData(Solution instance, ExtraPay extraPay)
        {
            Instance = instance;
            ExtraPay = extraPay;
        }
    }



}
