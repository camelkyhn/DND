using DND.Middleware.Dtos.Identity.Accounts;
using DND.Web.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using AreaConstants = DND.Middleware.Constants.Areas;

namespace DND.Web.Server.Areas.Identity.Controllers
{
    [Area(AreaConstants.Identity)]
    [Route("[area]/[controller]/[action]")]
    public class AccountController : BaseController
    {
        [HttpGet]
        public RegisterDto Register(RegisterDto dto)
        {
            return dto;
        }
    }
}
