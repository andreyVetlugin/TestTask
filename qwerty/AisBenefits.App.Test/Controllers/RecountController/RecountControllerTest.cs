using AisBenefits.Models.Reestrs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using AisBenefits.Models.Recounts;
using DataLayer.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.App.Test.Controllers.RecountController
{
    public class RecountControllerTest
    {

        [Test]
        public async Task First()
        {
            var data = TestData.Create()
                    .AddFilledCatalog()
                    .AddFirstRecountData()
                ;

            using (var context = new RecountControllerContext(data))
            {
                var result = context.Controller.Confirm(new RecountForm
                {
                    Date = new DateTime(2018,11,25),
                    ExtraPension = 1000,
                    GosPension = 1000,
                    PersonInfoRootId = RecountData.PersonInfoId,
                    Summ = 10000
                });

                await result.ExecuteResultAsync(context.ControllerContext.ActionContext);

                context.ControllerContext.HttpContext.Response.Body.Position = 0;

                Extra.Assert.StatusCode(200, context.ControllerContext.HttpContext.Response);

                var reestrElem = data.Get<RecountDebt>().AsQueryable()
                    .ByPersonRootId(RecountData.PersonInfoId)
                    .FirstOrDefault();

                Assert.IsNotNull(reestrElem);
                Assert.AreEqual(-680, reestrElem.Debt);
                Assert.AreEqual(7400, reestrElem.MonthlyPay);
            }
        }

    }
}
