using Microsoft.AspNetCore.Mvc;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Services;
using AisBenefits.Services.MassRecalculates;

namespace AisBenefits.Controllers
{
    [Route("/api/massrecalculate")]
    [ApiErrorHandler, ApiController]
    public class MassRecalculateController: Controller
    {
        private readonly BenefitsAppContext benefitsAppContext;

        public MassRecalculateController(BenefitsAppContext benefitsAppContext)
        {
            this.benefitsAppContext = benefitsAppContext;
        }

        [Route("recalculate")]
        public IActionResult Recalculate(MassRecalculateRecalculateForm form)
        {
            var result = benefitsAppContext.MassRecalculate(form);

            return new ApiOperationResult(result);
        }
    }
}
