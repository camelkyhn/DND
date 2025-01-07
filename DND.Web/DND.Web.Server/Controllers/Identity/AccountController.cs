using System.Threading.Tasks;
using DND.Business.Services.Identity;
using DND.Middleware.Constants;
using DND.Middleware.Dtos.Identity.Accounts;
using DND.Middleware.ResultDtos.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DND.Web.Server.Controllers.Identity
{
    [Area(Areas.Identity)]
    [Route("[area]/[controller]/[action]")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<LoginResultDto> Login(LoginDto dto)
        {
            var result = await _accountService.LoginAsync(dto);
            return result.Data;
        }
    }
}