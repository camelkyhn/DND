using System.Collections.Generic;
using System.Threading.Tasks;
using DND.Business.Services.Identity;
using DND.Middleware.Constants;
using DND.Middleware.Dtos.Identity.Users;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using Microsoft.AspNetCore.Mvc;

namespace DND.Web.Server.Controllers.Identity
{
    [Area(Areas.Identity)]
    [Route("[area]/[controller]/[action]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<Result<GetUserForViewDto>> Get(int id)
        {
            return await _userService.GetUserForViewAsync(id);
        }

        [HttpGet]
        public async Task<Result<List<GetUserForViewDto>>> GetList(UserFilterDto filterDto)
        {
            return await _userService.GetListAsync(filterDto);
        }

        [HttpPost]
        public async Task<Result<UserDto>> CreateOrUpdate(CreateOrUpdateUserDto dto)
        {
            return await _userService.CreateOrUpdateAsync(dto);
        }
    }
}