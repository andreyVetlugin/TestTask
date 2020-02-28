using System;
using System.Linq;
using AisBenefits.Infrastructure.Services.Integrations.DocsVision;
using Microsoft.AspNetCore.Mvc;
using AisBenefits.Models.Integrations.DocsVision;
using AisBenefits.Attributes;

namespace AisBenefits.Controllers
{
    [Route("api/public")]
    [ApiController]
    public class IntegrationController : Controller
    {
        private readonly IDvIntergationService dvIntergationService;

        public IntegrationController(IDvIntergationService dvIntergationService)
        {
            this.dvIntergationService = dvIntergationService;
        }

        [Route("payments")]
        [HttpPost]
        public JsonResult GetPaymentByPerson(DvPaymentForm form)
        {
            try
            {
                var resultDTO = dvIntergationService.GetPaymentsByPersonData(
                    form.PersonLN,
                    form.PersonFN,
                    form.PersonMN,
                    DateTime.TryParse(form.BirthDate, out var d) ? (DateTime?) d : null,
                    DateTime.TryParse(form.StartDate, out d) ? d : throw new Exception("не заполнено поле StartDate"),
                    DateTime.TryParse(form.EndDate, out d) ? d : throw new Exception("не заполнено поле EndDate"));
                return Json(new DvPaymentModel
                {
                    Status = DvResponseStatus.Single,
                    PaymentSum = resultDTO.PaymentSum,
                    BirthDate = resultDTO.BirthDate,
                    Udergania = resultDTO.Udergania,
                    Payments = resultDTO.Payments
                        .Select(x => new DvPayment {PayDate = x.PayDate, PaySum = x.PaySum})
                        .ToArray()
                });
            }
            catch (NotFoundException)
            {
                return Json(new DvPaymentModel {Status = DvResponseStatus.NotFound});
            }
            catch (TooManyPeopleException)
            {
                return Json(new DvPaymentModel {Status = DvResponseStatus.Many});
            }
            catch (Exception e)
            {
                /* TODO - это вообще законно?! */
                Response.StatusCode = 500;
                return Json(new DvPaymentModel {ErrMess = e.Message});
            }
        }
    }
}