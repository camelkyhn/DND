using System.Collections.Generic;
using System.Threading.Tasks;
using DND.Business.Services.Identity;
using DND.Middleware.Constants;
using DND.Middleware.Dtos.Identity.Permissions;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using Microsoft.AspNetCore.Mvc;

namespace DND.Web.Server.Controllers.Identity
{
    [Area(Areas.Identity)]
    [Route("[area]/[controller]/[action]")]
    public class PermissionController : AppController
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<Result<GetPermissionForViewDto>> Get(short id)
        {
            return await _permissionService.GetPermissionForViewAsync(id);
        }

        [HttpGet]
        public async Task<Result<List<GetPermissionForViewDto>>> GetList(PermissionFilterDto filterDto)
        {
            return await _permissionService.GetListAsync(filterDto);
        }

        [HttpPost]
        public async Task<Result<PermissionDto>> CreateOrUpdate([FromBody] CreateOrUpdatePermissionDto dto)
        {
            return await _permissionService.CreateOrUpdateAsync(dto);
        }

        [HttpGet]
        public async Task<Result> Delete(short id)
        {
            return await _permissionService.DeleteAsync(id);
        }
    }
}