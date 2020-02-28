using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisBenefits.ActionResults;
using AisBenefits.Attributes;
using AisBenefits.Attributes.PermAuth2;
using AisBenefits.Infrastructure.Permissions;
using AisBenefits.Infrastructure.Services;
using AisBenefits.Models.EGISSO.Privileges;
using AisBenefits.Services.EGISSO.Privileges;
using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using DataLayer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AisBenefits.Controllers
{
    [Route("api/EgissoPrivilege")]
    [ApiController, ApiErrorHandler]
    //[PermissionFilter(RolePermissions.AdministratePayouts)]
    public class EgissoPrivilegeController : ControllerBase
    {
        private readonly IPrivilegeEditCreateFormHandler privilegeEditCreateFormHandler;
        private readonly IPrivilegeEditEditFormHandler privilegeEditEditFormHandler;
        private readonly IReadDbContext<IBenefitsEntity> readDbContext;

        public EgissoPrivilegeController(IPrivilegeEditCreateFormHandler privilegeEditCreateFormHandler, IPrivilegeEditEditFormHandler privilegeEditEditFormHandler, IReadDbContext<IBenefitsEntity> readDbContext)
        {
            this.privilegeEditCreateFormHandler = privilegeEditCreateFormHandler;
            this.privilegeEditEditFormHandler = privilegeEditEditFormHandler;
            this.readDbContext = readDbContext;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(PrivilegeEditForm formHolder)
        {
            var result = privilegeEditCreateFormHandler.Handle(formHolder, ModelState);

            return new ApiOperationResult(result);
        }


        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            //var result = 
            //var privelegies = ModelDataResult<List<Privilege>>.BuildSucces(readDbContext.Get<Privilege>().ToList());
            var privelegies = readDbContext.Get<Privilege>().ToList()
                                            .Select(c=>
                                            {
                                                var link = readDbContext.Get<KpCodeLink>()
                                                    .Where(f => f.PrivilegeId == c.Id && f.Active).ToList();
                                                return new
                                                {
                                                    Privilege = c,
                                                    Categories = link
                                                };
                                            });
            
            return new ApiResult(privelegies);
        }



        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(PrivilegeEditForm formHolder)
        {
            var result = privilegeEditEditFormHandler.Handle(formHolder, ModelState);

            return new ApiOperationResult(result);
        }
    }
}