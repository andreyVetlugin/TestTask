using AisBenefits.Models.Solutions;
using AutoMapper;
using DataLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AisBenefits.Services.Solutions
{
    public interface ISolutionModelBuilder
    {
        List<SolutionModel> Build(List<Solution> solutions, Reestr lastReestr);
    }
    public class SolutionModelBuilder : ISolutionModelBuilder
    {
        public List<SolutionModel> Build(List<Solution> solutions, Reestr lastReestr)
        {
            return solutions
                .OrderByDescending(s => s.Execution)
                .ThenBy(s => s.OutdateTime.HasValue)
                .ThenByDescending(s => s.OutdateTime)
                .Select(s =>
                {
                    var r = Mapper.Map<Solution, SolutionModel>(s);
                    r.SolutionType_str = Solution.ConvertTypeToString(s.Type);
                    r.AllowDelete = s.AllowDelete(lastReestr);
                    return r;
                })
                .ToList();
        }
    }
}
