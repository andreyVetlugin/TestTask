using System;
using System.Collections.Generic;
using DataLayer.Entities;

namespace AisBenefits.App.Test.Controllers.ExtraPayController
{
    class ExtraPayControllerContext : IDisposable
    {
        public readonly AisBenefits.Controllers.ExtraPayController Controller;

        public readonly ControllerTestContext ControllerContext;

        public ExtraPayControllerContext(ICollection<IBenefitsEntity> data)
        {
            ControllerContext = new ControllerTestContext(
                sp => sp.GetHashCode(),
                data
                );

            Controller = ControllerContext.GetController<AisBenefits.Controllers.ExtraPayController>();
        }

        public void Dispose()
        {
            ControllerContext.Dispose();
        }
    }
}
