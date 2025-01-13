using System.Collections.Generic;
using System.Threading.Tasks;
using DND.Business.Services.Identity;
using DND.Middleware.Constants;
using DND.Middleware.Dtos.Identity.Roles;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using Microsoft.AspNetCore.Mvc;

namespace DND.Web.Server.Controllers.Identity
{
    [Area(Areas.Identity)]
    [Route("[area]/[controller]/[action]")]
    public class RoleController : AppController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<Result<GetRoleForViewDto>> Get(short id)
        {
            return await _roleService.GetRoleForViewAsync(id);
        }

        [HttpGet]
        public async Task<Result<List<GetRoleForViewDto>>> GetList(RoleFilterDto filterDto)
        {
            return await _roleService.GetListAsync(filterDto);
        }

        [HttpPost]
        public async Task<Result<RoleDto>> CreateOrUpdate([FromBody] CreateOrUpdateRoleDto dto)
        {
            return await _roleService.CreateOrUpdateAsync(dto);
        }

        [HttpGet]
        public async Task<Result> Delete(short id)
        {
            return await _roleService.DeleteAsync(id);
        }
    }
}