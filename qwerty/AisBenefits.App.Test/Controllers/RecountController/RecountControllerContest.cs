using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.App.Test.Controllers.RecountController
{
    class RecountControllerContext : IDisposable
    {
        public readonly AisBenefits.Controllers.RecountController Controller;

        public readonly ControllerTestContext ControllerContext;

        public RecountControllerContext(ICollection<IBenefitsEntity> data)
        {
            ControllerContext = new ControllerTestContext(
                sp => sp.GetHashCode(),
                data
            );

            Controller = ControllerContext.GetController<AisBenefits.Controllers.RecountController>();
        }


        public void Dispose()
        {
            ControllerContext.Dispose();
        }
    }
}
