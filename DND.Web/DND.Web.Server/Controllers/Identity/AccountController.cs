using System.Threading.Tasks;
using DND.Business.Services.Identity;
using DND.Middleware.Constants;
using DND.Middleware.Dtos.Identity.Accounts;
using DND.Middleware.ResultDtos.Identity;
using DND.Middleware.System;
using DND.Web.Server.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DND.Web.Server.Controllers.Identity
{
    [Area(Areas.Identity)]
    [Route("[area]/[controller]/[action]")]
    public class AccountController : AppController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<LoginResultDto>> Login([FromBody] LoginDto dto)
        {
            return await _accountService.LoginAsync(dto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<Result> Register([FromBody] RegisterDto dto)
        {
            return await _accountService.RegisterAsync(dto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<Result> ConfirmEmail([FromBody] ConfirmEmailDto dto)
        {
            return await _accountService.ConfirmEmailAsync(dto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<Result> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            return await _accountService.ForgotPasswordAsync(dto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<Result> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            return await _accountService.ResetPasswordAsync(dto);
        }

        [HttpPost]
        [AppAuthorize(Permissions.IdentityAccountChangePassword)]
        public async Task<Result> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            return await _accountService.ChangePasswordAsync(dto);
        }
    }
}