using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.App.Test.Controllers.ReestrController
{
    class ReestrControllerContext : IDisposable
    {
        public readonly AisBenefits.Controllers.ReestrController Controller;

        public readonly ControllerTestContext ControllerContext;

        public ReestrControllerContext(ICollection<IBenefitsEntity> data)
        {
            ControllerContext = new ControllerTestContext(
                sp => sp.GetHashCode(),
                data
            );

            Controller = ControllerContext.GetController<AisBenefits.Controllers.ReestrController>();
        }
       

        public void Dispose()
        {
            ControllerContext.Dispose();
        }
    }
}