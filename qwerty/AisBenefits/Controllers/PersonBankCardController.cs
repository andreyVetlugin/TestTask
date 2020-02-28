using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.Logging;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services.PersonInfos;
using AisBenefits.Models;
using AisBenefits.Models.PersonInfos;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/PersonBankCard")]
    [ApiController, ApiErrorHandler]
    [PermissionFilter(RolePermissions.AdministratePersonInfos)]
    public class PersonBankCardController : ControllerBase
    {
        private readonly IPersonBankCardService personBankCardService;

        public PersonBankCardController(IPersonBankCardService personBankCardService)
        {
            this.personBankCardService = personBankCardService;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(PersonBankCardForm personBankCardForm)
        {
            var res = personBankCardService.Create(personBankCardForm);
            return new ApiOperationResult(res);
        }

        [HttpPost]
        [Route("Get")]
        [GetLogging]
        public IActionResult Get(GuidIdForm guidIdForm)
        {
            var cardResult = personBankCardService.Get(guidIdForm.Id);

            return ApiModelResult.Create(cardResult, card =>
                 new PersonBankCardModel
                 {
                     Number = card.Number,
                     Id = card.Id,
                     PersonRootId = card.PersonRootId,
                     Type = card.Type,
                     ValidThru = card.ValidThru
                 });
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update(PersonBankCardForm personBankCardForm)
        {
            var res = personBankCardService.Update(personBankCardForm);
            return new ApiOperationResult(res);
        }


    }
}