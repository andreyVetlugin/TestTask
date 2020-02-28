using DataLayer.Entities;
using OldDataImport.App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldDataImport.App
{
    public static class SolutionDbfMapper
    {
        public static List<Solution> Map(PersonInfo personInfo, IEnumerable<IReshenie> reshenies)
        {
            var solutions = reshenies.Select(r =>
                new Solution
                {
                    Id = Guid.NewGuid(),
                    PersonInfoRootId = personInfo.RootId,
                    CreateTime = r.DT_RESH,
                    OutdateTime = null,

                    TotalExtraPay = r.DOPLATA,
                    TotalPension = r.PENS,
                    DS = r.MDS_PROC,
                    Comment = null,
                    DSperc = r.PROC_DS,
                    
                    Destination = r.DT_RESH,
                    Execution = r.DT_ISPOLN,
                    Type = r.VID_RESH == 4
                        ? SolutionType.Stop
                        : r.VID_RESH == 3
                            ? SolutionType.Resume
                            : r.VID_RESH != 1
                                ? SolutionType.Pause
                                : SolutionType.Opredelit
                })
                .OrderBy(s => s.Execution)
                .ToList();

            foreach (var solution in solutions.Skip(1))
                solution.Type = solution.Type == SolutionType.Opredelit ? SolutionType.Pereraschet : solution.Type;

            return solutions;
        }

        public static Solution Map(PersonInfo personInfo, IReshenie r)
        {
            return new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = personInfo.RootId,
                CreateTime = r.DT_RESH,
                OutdateTime = null,

                TotalExtraPay = r.DOPLATA,
                TotalPension = r.PENS,
                DS = r.MDS_PROC,
                Comment = null,
                DSperc = r.PROC_DS,

                Destination = r.DT_RESH,
                Execution = r.DT_ISPOLN,
                Type = r.VID_RESH == 4
                    ? SolutionType.Stop
                    : r.VID_RESH == 3
                        ? SolutionType.Resume
                        : r.VID_RESH != 1
                            ? SolutionType.Pause
                            : SolutionType.Opredelit
            };
        }
    }
}
