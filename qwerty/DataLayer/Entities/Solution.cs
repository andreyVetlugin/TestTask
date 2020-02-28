using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Entities
{
    public class Solution : IBenefitsEntity
    {
        public Guid Id { get; set; }
        public Guid PersonInfoRootId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? OutdateTime { get; set; }

        public DateTime Destination { get; set; }
        public DateTime Execution { get; set; }
        public SolutionType Type { get; set; }
        public string Comment { get; set; }

        public decimal TotalPension { get; set; }
        public decimal TotalExtraPay { get; set; }
        public decimal DS { get; set; }
        public decimal DSperc { get; set; }

        public bool IsElite { get; set; }


        public static string ConvertTypeToString(SolutionType type)
        {
            switch (type)
            {
                case SolutionType.Opredelit:
                    {
                        return "Определить";
                    };
                case SolutionType.Pause:
                    {
                        return "Приостановить";
                    };
                case SolutionType.Pereraschet:
                    {
                        return "Перерасчёт";
                    };
                case SolutionType.Resume:
                    {
                        return "Возобновить";
                    };
                case SolutionType.Stop:
                    {
                        return "Прекратить";
                    };

                default:
                    //return string.Empty;
                    throw new Exception();
            }


        }
    }

    public enum SolutionType : byte
    {
        Opredelit = 10,
        Pereraschet = 11,

        Pause = 20,
        Resume = 21,

        Stop = 30,

        Reject = 40
    }
   


    public static class SolutionQueryableExtensions
    {
        public static IQueryable<Solution> ById(this IQueryable<Solution> solutions, Guid id) =>
            solutions.Where(s => s.Id == id);
        public static IQueryable<Solution> ByPersonRootId(this IQueryable<Solution> solutions, Guid personRootId) =>
            solutions.Where(p => p.PersonInfoRootId == personRootId);

        public static IQueryable<Solution> ByPersonRootIdAndSolutionType(this IQueryable<Solution> solutions, Guid personRootId, SolutionType type) =>
            solutions.Where(p => p.PersonInfoRootId == personRootId&& p.Type==type);

        public static IQueryable<Solution> LastByPersonInfoRootIds(this IQueryable<Solution> solutions, IEnumerable<Guid> personInfoRootIds) =>
            solutions.Where(p => personInfoRootIds.Contains(p.PersonInfoRootId) && !p.OutdateTime.HasValue);

        public static IQueryable<Solution> Positive(this IQueryable<Solution> solutions) =>
           solutions.Where(sol =>  sol.Type != SolutionType.Stop && sol.Type != SolutionType.Pause && !sol.OutdateTime.HasValue);

        public static IQueryable<Solution> Actual(this IQueryable<Solution> query) =>
            query.Where(s => !s.OutdateTime.HasValue);

        public static IQueryable<Solution> Archive(this IQueryable<Solution> solutions) =>
            solutions.Where(sol => (sol.Type == SolutionType.Stop) && (!sol.OutdateTime.HasValue));

        public static IQueryable<Solution> TheLastByPersonRootId(this IQueryable<Solution> solutions, Guid personRootId) =>
            solutions.Where(p => p.PersonInfoRootId == personRootId && !p.OutdateTime.HasValue);

        public static IQueryable<Solution> Paused(this IQueryable<Solution> query) =>
            query.Where(s => s.Type == SolutionType.Pause);

    }

    public static class SolutionExtensions
    {
        public static bool AllowDelete(this Solution solution, Reestr lastReestr) =>
            solution.AllowDelete(lastReestr.Date);

        public static bool AllowDelete(this Solution solution, DateTime lastReestrDate) =>
            solution.Execution.Date > lastReestrDate.Date;

        public static bool IsPositive(this Solution solution) =>
            solution.Type == SolutionType.Opredelit ||
            solution.Type == SolutionType.Pereraschet ||
            solution.Type == SolutionType.Resume;
    }
}
