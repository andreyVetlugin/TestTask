using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Models.Reestrs;
using DataLayer.Entities;
using System.Linq;
using AisBenefits.Models;

namespace AisBenefits.App.Test.Controllers.ReestrController
{
    public class ReestrControllerTest
    {
        [Test]
        public async Task First()
        {
            var data = TestData.Create()
                    .AddFilledCatalog()
                    .AddFirstReestrData()
                ;
            var testDate = data.Get<Solution>().AsQueryable().Positive().FirstOrDefault().Execution;
            using (var context = new ReestrControllerContext(data))
            {
                var result = context.Controller.Init(new ReestrForm
                {
                    Date = testDate,
                    InitDate = testDate

                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                context.ControllerContext.HttpContext.Response.Body.Position = 0;

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var reestr = data.Get<Reestr>().AsQueryable()
                    .ByDate(testDate)
                    .FirstOrDefault();
                var reestrElem = data.Get<ReestrElement>().AsQueryable()
                    .ByPersonInfoRootId(ReestrData.PersonInfoId)
                    .ByReestrId(reestr.Id)
                    .FirstOrDefault();

                Assert.IsNotNull(reestrElem);
                Assert.AreEqual(1000, reestrElem.Summ);
                Assert.AreEqual(false, reestrElem.Deleted);
            }
        }

        [Test]
        public async Task Second()
        {
            var data = TestData.Create()
                    .AddFilledCatalog()
                    .AddSecondReestrData()
                ;

            using (var context = new ReestrControllerContext(data))
            {
                var result = context.Controller.Init(new ReestrForm
                {
                    Date = new DateTime(2019, 4, 27),
                    InitDate = new DateTime(2019, 4, 27)

                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                context.ControllerContext.HttpContext.Response.Body.Position = 0;

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var reestr = data.Get<Reestr>().AsQueryable()
                    .ByDate(new DateTime(2019, 4, 27))
                    .FirstOrDefault();
                var reestrElem = data.Get<ReestrElement>().AsQueryable()
                    .ByPersonInfoRootId(ReestrData.PersonInfoId)
                    .ByReestrId(reestr.Id)
                    .FirstOrDefault();

                Assert.IsNotNull(reestrElem);
                Assert.AreEqual(1500, reestrElem.Summ);
                Assert.AreEqual(false, reestrElem.Deleted);
            }
        }

        [Test]
        public async Task Third()
        {
            var data = TestData.Create()
                    .AddFilledCatalog()
                    .AddThirdReestrData()
                ;

            using (var context = new ReestrControllerContext(data))
            {
                var result = context.Controller.Init(new ReestrForm
                {
                    Date = new DateTime(2019, 4, 27),
                    InitDate = new DateTime(2019, 4, 27)

                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                context.ControllerContext.HttpContext.Response.Body.Position = 0;

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var reestr = data.Get<Reestr>().AsQueryable()
                    .ByDate(new DateTime(2019, 4, 27))
                    .FirstOrDefault();
                var reestrElem = data.Get<ReestrElement>().AsQueryable()
                    .ByPersonInfoRootId(ReestrData.PersonInfoId)
                    .ByReestrId(reestr.Id)
                    .FirstOrDefault();

                Assert.IsNotNull(reestrElem);
                Assert.AreEqual(500, reestrElem.Summ);
                Assert.AreEqual(false, reestrElem.Deleted);
            }
        }

        [Test]
        public async Task Fourth()
        {
            var data = TestData.Create()
                    .AddFilledCatalog()
                    .AddFourthReestrData()
                ;

            using (var context = new ReestrControllerContext(data))
            {
                var result = context.Controller.Init(new ReestrForm
                {
                    Date = new DateTime(2019, 4, 27),
                    InitDate = new DateTime(2019, 4, 27)

                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                context.ControllerContext.HttpContext.Response.Body.Position = 0;

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var reestrElem = data.Get<ReestrElement>().AsQueryable()
                    .ByPersonInfoRootId(ReestrData.PersonInfoId)
                    .FirstOrDefault();

                Assert.IsNull(reestrElem);
            }
        }



        [Test]
        public async Task Fivth()
        {
            var data = TestData.Create()
                    .AddFilledCatalog()
                    .AddFivthReestrData()
                ;

            using (var context = new ReestrControllerContext(data))
            {
                var result = context.Controller.Init(new ReestrForm
                {
                    Date = new DateTime(2019, 4, 27),
                    InitDate = new DateTime(2019, 4, 27)

                });
                
                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                context.ControllerContext.HttpContext.Response.Body.Position = 0;

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var reestrElem = data.Get<ReestrElement>().AsQueryable()
                    .ByPersonInfoRootId(ReestrData.PersonInfoId)
                    .LastOrDefault();

                var debt = data.Get<RecountDebt>().AsQueryable()
                    .ByPersonRootId(ReestrData.PersonInfoId)
                    .FirstOrDefault();
                
                Assert.AreEqual(500, reestrElem.Summ);
            }
        }

        [Test]
        public async Task NoPreviewMonthPayShouldGetSecondPartialPay()
        {
            var data = TestData.Create()
                .AddFilledCatalog()
                .AddReadyToPayPerson(out var personInfo)
                .AddSolution(out var initialSolution,
                    new DateTime(2019, 3, 27),
                    false,
                    SolutionType.Opredelit,
                    930)
                .AddSolution(out var actiualSolution,
                    new DateTime(2019, 4, 1),
                    true,
                    SolutionType.Pereraschet,
                    1000);
            using (var context = new ReestrControllerContext(data))
            {
                var result = context.Controller.Init(new ReestrForm
                {
                    Date = new DateTime(2019, 4, 27),
                    InitDate = new DateTime(2019, 4, 27)

                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                context.ControllerContext.HttpContext.Response.Body.Position = 0;

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var reestrElem = data.Get<ReestrElement>().AsQueryable()
                    .ByPersonInfoRootId(ReestrData.PersonInfoId)
                    .LastOrDefault();

                Assert.AreEqual(1150, reestrElem.Summ);
            }
        }
        [Test]
        public async Task PausedNoPreviewMonthPayAndResumeShoudnotGetSecondPay()
        {
            var data = TestData.Create()
                .AddFilledCatalog()
                .AddReadyToPayPerson(out var personInfo)
                .AddSolution(out var initialSolution,
                    new DateTime(2019, 2, 1),
                    false,
                    SolutionType.Opredelit,
                    1000)
                .AddSolution(out var pausedSolution,
                    new DateTime(2019, 3, 1),
                    false,
                    SolutionType.Pause,
                    930)
                .AddSolution(out var actiualSolution,
                    new DateTime(2019, 4, 1),
                    true,
                    SolutionType.Resume,
                    1000);
            using (var context = new ReestrControllerContext(data))
            {
                var result = context.Controller.Init(new ReestrForm
                {
                    Date = new DateTime(2019, 4, 27),
                    InitDate = new DateTime(2019, 4, 27)

                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                context.ControllerContext.HttpContext.Response.Body.Position = 0;

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var reestrElem = data.Get<ReestrElement>().AsQueryable()
                    .ByPersonInfoRootId(ReestrData.PersonInfoId)
                    .LastOrDefault();

                Assert.AreEqual(1000, reestrElem.Summ);
            }
        }
        [Test]
        public async Task ResumeInMiddleMonthPausedShouldGetPartialPayWithNoPreview()
        {
            var data = TestData.Create()
                .AddFilledCatalog()
                .AddReadyToPayPerson(out var personInfo)
                .AddSolution(out var initialSolution,
                    new DateTime(2019, 2, 1),
                    false,
                    SolutionType.Opredelit,
                    1000)
                .AddSolution(out var pausedSolution,
                    new DateTime(2019, 3, 1),
                    false,
                    SolutionType.Pause,
                    930)
                .AddSolution(out var actiualSolution,
                    new DateTime(2019, 4, 16),
                    true,
                    SolutionType.Resume,
                    1000);
            using (var context = new ReestrControllerContext(data))
            {
                var result = context.Controller.Init(new ReestrForm
                {
                    Date = new DateTime(2019, 4, 27),
                    InitDate = new DateTime(2019, 4, 27)

                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                context.ControllerContext.HttpContext.Response.Body.Position = 0;

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var reestrElem = data.Get<ReestrElement>().AsQueryable()
                    .ByPersonInfoRootId(ReestrData.PersonInfoId)
                    .LastOrDefault();

                Assert.AreEqual(500, reestrElem.Summ);
            }
        }

    }
}
