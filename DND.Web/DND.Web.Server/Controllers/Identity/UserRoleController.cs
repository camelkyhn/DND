using System.Collections.Generic;
using System.Threading.Tasks;
using DND.Business.Services.Identity;
using DND.Middleware.Constants;
using DND.Middleware.Dtos.Identity.UserRoles;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using Microsoft.AspNetCore.Mvc;

namespace DND.Web.Server.Controllers.Identity
{
    [Area(Areas.Identity)]
    [Route("[area]/[controller]/[action]")]
    public class UserRoleController : AppController
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet]
        public async Task<Result<GetUserRoleForViewDto>> Get(long id)
        {
            return await _userRoleService.GetUserRoleForViewAsync(id);
        }

        [HttpGet]
        public async Task<Result<List<GetUserRoleForViewDto>>> GetList(UserRoleFilterDto filterDto)
        {
            return await _userRoleService.GetListAsync(filterDto);
        }

        [HttpPost]
        public async Task<Result<UserRoleDto>> CreateOrUpdate([FromBody] CreateOrUpdateUserRoleDto dto)
        {
            return await _userRoleService.CreateOrUpdateAsync(dto);
        }

        [HttpGet]
        public async Task<Result> Delete(long id)
        {
            return await _userRoleService.DeleteAsync(id);
        }
    }
}