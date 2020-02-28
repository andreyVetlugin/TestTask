using System;
using System.Collections.Generic;
using System.Linq;
using AisBenefits.Infrastructure.DTOs;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.Infrastructure.Services.Solutions
{
    public class SolutionService : ISolutionService
    {

        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IExtraPayCountService extraPayCountService;
        private readonly ILogBuilder logBuilder;

        public SolutionService(IReadDbContext<IBenefitsEntity> readDbContext, IWriteDbContext<IBenefitsEntity> writeDbContext, IExtraPayCountService extraPayCountService, ILogBuilder logBuilder)
        {
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
            this.extraPayCountService = extraPayCountService;
            this.logBuilder = logBuilder;
        }

        public void Count(ISolutionForm solutionForm)
        {
            var sol = CreateEntity(solutionForm, SolutionType.Pereraschet);

            writeDbContext.Add(sol);

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.SolutionPereraschet, sol.Id);

            writeDbContext.Add(infoLog);

            writeDbContext.SaveChanges();
        }

        public OperationResult CountFromPensionUpdate(ExtraPay extraPay, DateTime destination, DateTime execution, string comment, bool isElite = false)
        {
            var sol = CreateEntity(extraPay, destination, execution, comment, SolutionType.Pereraschet);

            sol.IsElite = isElite;

            writeDbContext.Add(sol);

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.SolutionPereraschet, sol.Id);

            writeDbContext.Add(infoLog);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
        
        public OperationResult<Guid> DeleteSolution(Guid solutionId, out bool isSolutionActive)
        {
            var sol = readDbContext.Get<Solution>().ById(solutionId).FirstOrDefault();

            var personRootId = sol.Id;

            var prevSol = readDbContext.Get<Solution>().ByPersonRootId(sol.PersonInfoRootId)
                .Where(c => c.Id!=sol.Id && c.Execution <= sol.Execution).OrderByDescending(c => c.Execution)
                .FirstOrDefault();

            var prevSolisNull = prevSol == null;

            var theLastPaymentDate = readDbContext.Get<Reestr>().Max(c => c.InitDate).Date;

            isSolutionActive = prevSolisNull ? true:
                prevSol.Type == SolutionType.Opredelit || prevSol.Type == SolutionType.Pereraschet ||
                prevSol.Type == SolutionType.Resume
                    ? true
                    : false;
            
            if (!sol.AllowDelete(theLastPaymentDate)) return OperationResult<Guid>.BuildFormError("Удаление невозможно, реестр с этим решением уже был составлен");
            
            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.DeleteSolution, sol.Id);

            writeDbContext.Add(infoLog);

            writeDbContext.Remove(sol);

            if (!prevSolisNull)
            {
                writeDbContext.Attach(prevSol);
                prevSol.OutdateTime = null;
            }
            
            return OperationResult<Guid>.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext), personRootId);
        }
        
        public List<Solution> Get(Guid personInfoRootId)
        {
            return readDbContext.Get<Solution>().ByPersonRootId(personInfoRootId).ToList();
        }

        public Solution GetLast(Guid personInfoRootId)
        {
            return readDbContext.Get<Solution>().TheLastByPersonRootId(personInfoRootId).FirstOrDefault();
        }
        
        public void Opredelit(ISolutionForm solutionForm)
        {
            var sol = CreateEntity(solutionForm, SolutionType.Opredelit);

            writeDbContext.Add(sol);

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.SolutionOpredelit, sol.Id);

            writeDbContext.Add(infoLog);

            writeDbContext.SaveChanges();
        }
        
        public void Pause(ISolutionForm solutionForm)
        {
            var sol = CreateEntity(solutionForm, SolutionType.Pause);

            writeDbContext.Add(sol);

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.SolutionPause, sol.Id);

            writeDbContext.Add(infoLog);

            writeDbContext.SaveChanges();
        }
        
        public void Resume(ISolutionForm solutionForm)
        {
            var sol = CreateEntity(solutionForm, SolutionType.Resume);

            writeDbContext.Add(sol);

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.SolutionResume, sol.Id);

            writeDbContext.Add(infoLog);

            writeDbContext.SaveChanges();
        }
        
        public void Stop(ISolutionForm solutionForm)
        {
            var sol = CreateEntity(solutionForm, SolutionType.Stop);

            writeDbContext.Add(sol);

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.SolutionStop, sol.Id);

            writeDbContext.Add(infoLog);

            writeDbContext.SaveChanges();
        }
        
        private Solution CreateEntity(ISolutionForm solutionForm, SolutionType type)
        {
            var sol = Mapper.Map<ISolutionForm, Solution>(solutionForm);

            var extraPays = extraPayCountService.Count(solutionForm.PersonInfoRootId);

            var minExtra = readDbContext.Get<MinExtraPay>().OrderByDescending(c=>c.Date).FirstOrDefault();


            //sol.DS = 12;
            //sol.DSperc = 1;
            //sol.TotalExtraPay = 5;
            //sol.TotalPension = 6;

            sol.Id = Guid.NewGuid();
            sol.DS = extraPays.TotalPensionAnExtraPay;
            sol.DSperc = extraPays.DsPerc;
           
            sol.TotalExtraPay = extraPays.TotalExtraPay < minExtra.Value? minExtra.Value : extraPays.TotalExtraPay;

            sol.TotalPension = extraPays.TotalPension;
            sol.Type = type;
            sol.CreateTime = DateTime.Now;

            var currentLast = readDbContext.Get<Solution>().TheLastByPersonRootId(solutionForm.PersonInfoRootId).FirstOrDefault();

            if (currentLast != null)
            {
                writeDbContext.Attach(currentLast);
                currentLast.OutdateTime = DateTime.Now;
            }

            return sol;
        }

        private Solution CreateEntity(ExtraPay extraPay, DateTime destination, DateTime execution, string comment, SolutionType type)
        {
            var sol = new Solution
            {
                Id = Guid.NewGuid(),
                Destination = destination,
                Execution = execution,
                Comment = comment,
                PersonInfoRootId = extraPay.PersonRootId,
                DS = extraPay.TotalPensionAndExtraPay,
                DSperc = extraPay.DsPerc,
                TotalExtraPay = extraPay.TotalExtraPay,
                TotalPension = extraPay.TotalPension,
                Type = type,
                CreateTime = DateTime.Now
            };
            
            var currentLast = readDbContext.Get<Solution>().TheLastByPersonRootId(extraPay.PersonRootId).FirstOrDefault();

            if (currentLast != null)
            {
                writeDbContext.Attach(currentLast);
                currentLast.OutdateTime = DateTime.Now;
            }

            return sol;
        }
    }
}
