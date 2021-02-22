using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SP.ControlPanel.Api.Models;
using SP.ControlPanel.Api.ViewModels;
using SP.ControlPanel.Business.Interfaces.Enums;
using SP.ControlPanel.Business.Interfaces.Model;
using SP.ControlPanel.Business.Interfaces.Services;

namespace SP.ControlPanel.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private IAccountsService _accountsService;

        public AccountController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World!");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post(AccountVM accountToCreate)
        {
            var failedValidations = accountToCreate.IsValid();
            if (failedValidations.Any())
            {
                return BadRequest(new { FailedValidations = failedValidations });
            }

            //TODO: Add user to authentication server
            string identityProviderId = Guid.NewGuid().ToString();

            IUserAccount userAccount = accountToCreate.ToUserAccount();

            userAccount.IdentityProviderId = identityProviderId;
            userAccount.AccountType = AccountTypes.RootAccount;

            IUserAccount createdAccount = _accountsService.Create(userAccount);

            return Ok(createdAccount);
        }

        [HttpPut]
        public IActionResult Put()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok();
        }
    }
}