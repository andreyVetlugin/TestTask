using System.Linq;
using AisBenefits.Models.ExtraPays;
using NUnit.Framework;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace AisBenefits.App.Test.Controllers.ExtraPayController
{
    public class ExtraPayControllerTest
    {
        [Test]
        public async Task GetShouldSuccess()
        {
            var data = TestData.Create()
                .AddFilledCatalog()
                .AddFullExtraPayData(CatalogData.OneXOrganization)
                ;

            using (var context = new ExtraPayControllerContext(data))
            {
                var result = context.Controller.Get(new ExtraPayGetForm
                {
                    PersonRootId = ExtraPayData.PersonInfoId
                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);
                context.ControllerContext.HttpContext.Response.Body.Position = 0;

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);
            }
        }

        [Test]
        public async Task EditShouldSuccessAndCreateOpredelitSolution()
        {
            var data = TestData.Create()
                .AddFilledCatalog()
                .AddInitialExtraPayData(CatalogData.OneXOrganization)
                ;
            using (var context = new ExtraPayControllerContext(data))
            {
                var result = context.Controller.Edit(
                    new ExtraPayEditForm
                    {
                        PersonRootId = ExtraPayData.PersonInfoId,
                        TotalExtraPay = 3000,
                        CreateSolution = true
                    });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var extraPay = data.Get<ExtraPay>().AsQueryable()
                    .ActualByPersonRootId(ExtraPayData.PersonInfoId)
                    .FirstOrDefault();

                Assert.AreEqual(3000, extraPay.TotalExtraPay);

                var solution = data.Get<Solution>().AsQueryable()
                    .TheLastByPersonRootId(ExtraPayData.PersonInfoId)
                    .FirstOrDefault();

                Assert.IsNotNull(solution);
                Assert.AreEqual(SolutionType.Opredelit, solution.Type);
            }
        }

        [Test]
        public async Task EditShouldSuccessAndCreatePereraschetSolution()
        {
            var data = TestData.Create()
                    .AddFilledCatalog()
                    .AddDefinedExtraPayData(CatalogData.OneXOrganization)
                ;
            using (var context = new ExtraPayControllerContext(data))
            {
                var result = context.Controller.Edit(
                    new ExtraPayEditForm
                    {
                        PersonRootId = ExtraPayData.PersonInfoId,
                        TotalExtraPay = 3000
                    });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var extraPay = data.Get<ExtraPay>().AsQueryable()
                    .ActualByPersonRootId(ExtraPayData.PersonInfoId)
                    .FirstOrDefault();

                Assert.AreEqual(3000, extraPay.TotalExtraPay);

                var solution = data.Get<Solution>().AsQueryable()
                    .TheLastByPersonRootId(ExtraPayData.PersonInfoId)
                    .FirstOrDefault();

                Assert.IsNotNull(solution);
                Assert.AreEqual(SolutionType.Pereraschet, solution.Type);
            }
        }

        [Test]
        public async Task EditPausedExtraPayShouldSuccessAndCreateResumeSolutionAndRestoreFromArchive()
        {
            var data = TestData.Create()
                    .AddFilledCatalog()
                    .AddPausedExtraPayData(CatalogData.OneXOrganization)
                ;
            using (var context = new ExtraPayControllerContext(data))
            {
                var result = context.Controller.Edit(
                    new ExtraPayEditForm
                    {
                        PersonRootId = ExtraPayData.PersonInfoId,
                        TotalExtraPay = 3000,
                        CreateSolution = true
                    });


                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var extraPay = data.Get<ExtraPay>().AsQueryable()
                    .ActualByPersonRootId(ExtraPayData.PersonInfoId)
                    .FirstOrDefault();

                Assert.AreEqual(3000, extraPay.TotalExtraPay);

                var solution = data.Get<Solution>().AsQueryable()
                    .TheLastByPersonRootId(ExtraPayData.PersonInfoId)
                    .FirstOrDefault();

                Assert.IsNotNull(solution);
                Assert.AreEqual(SolutionType.Resume, solution.Type);

                var personInfo = data.Get<PersonInfo>().AsQueryable()
                    .ByRootId(ExtraPayData.PersonInfoId)
                    .FirstOrDefault();
            }
        }
    }
}
