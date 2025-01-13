using System.Collections.Generic;
using System.Threading.Tasks;
using DND.Business.Services.Identity;
using DND.Middleware.Constants;
using DND.Middleware.Dtos.Identity.RolePermissions;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using Microsoft.AspNetCore.Mvc;

namespace DND.Web.Server.Controllers.Identity
{
    [Area(Areas.Identity)]
    [Route("[area]/[controller]/[action]")]
    public class RolePermissionController : AppController
    {
        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        [HttpGet]
        public async Task<Result<GetRolePermissionForViewDto>> Get(int id)
        {
            return await _rolePermissionService.GetRolePermissionForViewAsync(id);
        }

        [HttpGet]
        public async Task<Result<List<GetRolePermissionForViewDto>>> GetList(RolePermissionFilterDto filterDto)
        {
            return await _rolePermissionService.GetListAsync(filterDto);
        }

        [HttpPost]
        public async Task<Result<RolePermissionDto>> CreateOrUpdate([FromBody] CreateOrUpdateRolePermissionDto dto)
        {
            return await _rolePermissionService.CreateOrUpdateAsync(dto);
        }

        [HttpGet]
        public async Task<Result> Delete(int id)
        {
            return await _rolePermissionService.DeleteAsync(id);
        }
    }
}