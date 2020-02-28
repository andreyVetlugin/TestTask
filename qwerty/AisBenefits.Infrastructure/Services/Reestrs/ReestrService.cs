using AisBenefits.Infrastructure.DTOs;
using AisBenefits.Infrastructure.Services.PersonInfos;
using AisBenefits.Infrastructure.Services.Recounts;
using AisBenefits.Infrastructure.Services.Solutions;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AisBenefits.Infrastructure.Services.Reestrs
{
    public class ReestrService : IReestrService
    {
        private readonly IWriteDbContext<IBenefitsEntity> writeDbContext;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;
        private readonly ILogBuilder logBuilder;
        private readonly IExtraPayCountService extraPayCountService;

        public ReestrService(IWriteDbContext<IBenefitsEntity> writeDbContext, IReadDbContext<IBenefitsEntity> readDbContext, ILogBuilder logBuilder, IExtraPayCountService extraPayCountService)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
            this.logBuilder = logBuilder;
            this.extraPayCountService = extraPayCountService;
        }

        public Reestr Complete(Guid reestrId)
        {
            var reestr = readDbContext.Get<Reestr>().ByReestrId(reestrId).FirstOrDefault();
            if (reestr == null)
            {
                throw new Exception(message: "Нет такого реестра");
            }

            if (reestr.IsCompleted)
                return reestr;

            writeDbContext.Attach(reestr);
            reestr.IsCompleted = true;

            var elements = readDbContext.Get<ReestrElement>()
                .ByReestrId(reestrId)
                .ToList();

            var personInfoRootIds = elements.Select(c => c.PersonInfoRootId);

            var debtEntities = readDbContext.GetRecountDebtModelDataResult(personInfoRootIds);

            foreach (var debtEntity in debtEntities)
            {
                if (debtEntity.Data.IsCompletedInThisMonth) continue;
                writeDbContext.Attach(debtEntity.Data.RecountDebt);
                debtEntity.Data.RecountDebt.LastTimePay = DateTime.Now;
                debtEntity.Data.RecountDebt.Debt -= debtEntity.Data.CurrentPay;
                if (debtEntity.Data.RecountDebt.Debt == 0)
                    debtEntity.Data.RecountDebt.MonthlyPay = 0;
            }

            var personBankCards = readDbContext.Get<PersonBankCard>()
                .ActualByPersonInfoRootIds(elements.Select(e => e.PersonInfoRootId))
                .ToList();

            var pairs = (from element in elements
                         join bankCard in personBankCards on element.PersonInfoRootId equals bankCard.PersonRootId
                         select (element, bankCard))
                        .ToList();

            if (pairs.Count < elements.Count)
            {
                throw new InvalidOperationException("Не у всех пользователей указаны банковские реквизиты");
            }

            foreach (var (element, bankCard) in pairs)
            {
                writeDbContext.Attach(element);

                element.PersonBankCardId = bankCard.Id;
            }

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.ReestrComplete, reestrId);
            writeDbContext.Add(infoLog);

            writeDbContext.SaveChanges();
            return reestr;

        }



        public List<Reestr> GetArchive(int year, int month)
        {
            var reestrs = readDbContext.Get<Reestr>().CompletedByYearAndMonth(year, month).ToList();

            return reestrs;
        }

        public List<Reestr> GetArchive()
        {
            var reestrs = readDbContext.Get<Reestr>().Completed().ToList();
            return reestrs;
        }



        public Guid DeleteFromReestr(Guid reestrElemId)
        {
            var elem = readDbContext.Get<ReestrElement>().ById(reestrElemId).FirstOrDefault();
            if (elem == null)
            {
                throw new Exception(message: "Нет такого элемента");
            }


            var reestr = readDbContext.Get<Reestr>().ByReestrId(elem.ReestrId).FirstOrDefault();
            if (reestr.IsCompleted)
            {
                throw new Exception(message: "Реестр уже выгружен");
            }


            var reestrId = elem.ReestrId;
            writeDbContext.Attach(elem);
            elem.Deleted = true;

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.DeleteFromReestr, reestrId);
            writeDbContext.Add(infoLog);

            writeDbContext.SaveChanges();
            return reestrId;
        }


        public Reestr Init(IReestrDTO reestrDTO)
        {
            DeleteNotSavedReestrs(); //Удаляем нахер все невыгруженные реестры

            var reestr = Mapper.Map<IReestrDTO, Reestr>(reestrDTO);
            reestr.IsCompleted = false;
            reestr.Id = Guid.NewGuid();
            reestr.Number = readDbContext.Get<Reestr>().Count() > 0
                ? readDbContext.Get<Reestr>().Max(r => r.Number) + 1
                : 1;

            writeDbContext.Add(reestr);

            InitElements(reestr.Id, reestrDTO.Date);

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.ReestrCreate, reestr.Id);
            writeDbContext.Add(infoLog);



            writeDbContext.SaveChanges();

            return reestr;
        }

        public Reestr Get()
        {
            var notCompletedReestr = readDbContext.Get<Reestr>().NotCompleted().FirstOrDefault();
            return notCompletedReestr;
        }


        //public Reestr InitOrGet(IReestrDTO reestrDTO)
        //{
        //    DeleteNotSavedReestrs(); //Удаляем сначала все реестры и их элементы, которые без галочки saved

        //    var currentReestr = readDbContext.Get<Reestr>().NotCompletedByDate(reestrDTO.Date).Saved().FirstOrDefault();
        //    if (currentReestr != null)
        //    {
        //        return currentReestr;
        //    }

        //    var reestr = Mapper.Map<IReestrDTO, Reestr>(reestrDTO);
        //    reestr.IsCompleted = false;
        //    reestr.Id = Guid.NewGuid();
        //    reestr.Number = readDbContext.Get<Reestr>().Count()>0 
        //        ? readDbContext.Get<Reestr>().Max(r => r.Number) + 1
        //        : 1;

        //    writeDbContext.Add(reestr);

        //    InitElements(reestr.Id, reestrDTO.Date);

        //    var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.ReestrCreate, reestr.Id);
        //    writeDbContext.Add(infoLog);



        //    writeDbContext.SaveChanges();

        //    return reestr;
        //}

        public List<ReestrElement> GetReestrElements(Guid reestrId)
        {
            var list = readDbContext.Get<ReestrElement>().ByReestrId(reestrId).ToList();

            return list;
        }

        public Reestr GetReestrById(Guid reestrId)
        {
            var r = readDbContext.Get<Reestr>().ByReestrId(reestrId).FirstOrDefault();
            return r;
        }

        public Guid ReCountElementFromReestr(IRecountReestrElementDTO reestrElemForm, bool saveDb = true)
        {
            var elem = readDbContext.Get<ReestrElement>().ById(reestrElemForm.ReestrElementId).FirstOrDefault();

            if (elem == null)
            {
                throw new Exception(message: "Нет такого элемента");
            }

            var reestr = readDbContext.Get<Reestr>().ByReestrId(elem.ReestrId).FirstOrDefault();
            if (reestr.IsCompleted)
            {
                throw new Exception(message: "Реестр уже выгружен");
            }

            var debtPayEntity = readDbContext.GetRecountDebtModelDataResult(elem.PersonInfoRootId);
            var debtPay = debtPayEntity.Data.CurrentPay;


            writeDbContext.Attach(elem);
            elem.From = reestrElemForm.From;
            elem.To = reestrElemForm.To;
            elem.Summ = reestrElemForm.NewSumm + debtPay;

            var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.ReCountReestrElement, reestr.Id);
            writeDbContext.Add(infoLog);

            if (saveDb)
                writeDbContext.SaveChanges();

            return elem.ReestrId;
        }

        public Guid ReCountAllElementsFromReestr(IRecountReestrElementDTO[] reestrElemForms)
        {
            var reestrId = default(Guid);

            foreach (var reestrElemForm in reestrElemForms)
            {
                reestrId = ReCountElementFromReestr(reestrElemForm, false);
            }
            writeDbContext.SaveChanges();

            return reestrId;
        }

        //public OperationResult SaveReestr(Guid reestrId)
        //{
        //    var reestr = readDbContext.Get<Reestr>().ByReestrId(reestrId).FirstOrDefault();
        //    writeDbContext.Attach(reestr);
        //    reestr.IsSaved = true;
        //    return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        //}

        private void InitElements(Guid reestrId, DateTime date)
        {
            var personInfoRootIds = readDbContext.GetActivePersonInfoRootIds();

            var personInfoRootIdsWithAccount = readDbContext.Get<PersonBankCard>()
                .ActualByPersonInfoRootIds(personInfoRootIds)
                .Where(c => c.Type == PersonBankCardType.Account)
                .Select(c => c.PersonRootId)
                .ToHashSet();
            personInfoRootIds = Enumerable.Intersect(personInfoRootIds, personInfoRootIdsWithAccount)
                .ToHashSet();

            var personInfos = readDbContext.Get<PersonInfo>().ByRootIdsAndConfirmedExp(personInfoRootIds).ToList();

            var allSolutions = readDbContext.Get<Solution>()
                .AsEnumerable()
                .GroupBy(c => c.PersonInfoRootId)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(c => c.Execution.Date)
                        .Select(gc => gc.OrderBy(c => c.OutdateTime.HasValue).ThenByDescending(c => c.OutdateTime).First())
                        .OrderByDescending(s => s.Execution)
                        .ToList());

            var previewMonthDate = date.AddMonths(-1);
            var previewMonthReestrIds = readDbContext.Get<Reestr>()
                .Completed()
                .ByDate(previewMonthDate)
                .Select(r => r.Id)
                .ToList();

            var previewMonthPayoutPersonRootIds = allSolutions.Values
                .SelectMany(ss => ss.Select(s => s))
                .Where(s => s.Execution.Year <= previewMonthDate.Year &&
                            s.Execution.Month <= previewMonthDate.Month)
                .Select(s => s.PersonInfoRootId)
                .Except(
                    readDbContext.Get<ReestrElement>()
                        .ByReestrIds(previewMonthReestrIds)
                        .Select(p => p.PersonInfoRootId));

            foreach (var personInfo in personInfos)
            {
                //Все солюшны человека упорядоченные по дате исполнения (по убыванию)
                var eliteSolsDateList = allSolutions[personInfo.RootId].Where(c => c.IsElite)
                    .Select(c => (c.Execution, c.Destination));
                //var personAllSolutionsList = allSolutions[personInfo.RootId];
                var personAllSolutionsList = allSolutions[personInfo.RootId].Where(solution =>
                {
                    foreach (var dateTuple in eliteSolsDateList)
                    {
                        if (solution.Execution > dateTuple.Item1 && solution.Execution < dateTuple.Item2)
                            return false;
                    }
                    return true;
                });

                var sum = SolutionHelper.CalculateSummForMonth(personAllSolutionsList, date);
                var previewMonthSum = previewMonthPayoutPersonRootIds.Contains(personInfo.RootId)
                    ? SolutionHelper.CalculateSummForMonth(personAllSolutionsList, previewMonthDate)
                    : 0;

                var debtPayEntity = readDbContext.GetRecountDebtModelDataResult(personInfo.RootId);
                var debtPay = debtPayEntity.Data.CurrentPay;

                var temp = new ReestrElement
                {
                    Deleted = false,
                    Id = Guid.NewGuid(),
                    PersonInfoRootId = personInfo.RootId,
                    PersonInfoId = personInfo.Id,
                    ReestrId = reestrId,
                    BaseSumm = sum,
                    Summ = sum + previewMonthSum + debtPay
                };

                writeDbContext.Add(temp);

                var infoLog = logBuilder.BuildPostInfoLog(PostOperationType.CreateReestrElement, temp.Id);
                writeDbContext.Add(infoLog);
            }
        }

        private void DeleteNotSavedReestrs()
        {
            var reestrs = readDbContext.Get<Reestr>().NotCompleted().ToArray();
            var reestrElems = readDbContext.Get<ReestrElement>().ByReestrIdsIncludeDeleted(reestrs.Select(c => c.Id)).ToArray();

            writeDbContext.RemoveRange(reestrs);
            writeDbContext.RemoveRange(reestrElems);
        }


    }


}
