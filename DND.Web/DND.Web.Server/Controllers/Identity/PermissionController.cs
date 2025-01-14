using System.Collections.Generic;
using System.Threading.Tasks;
using DND.Business.Services.Identity;
using DND.Middleware.Constants;
using DND.Middleware.Dtos.Identity.Permissions;
using DND.Middleware.FilterDtos.Identity;
using DND.Middleware.System;
using DND.Web.Server.Attributes;
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
        [Route("{id}")]
        [AppAuthorize(Permissions.IdentityPermissionGet)]
        public async Task<Result<GetPermissionForViewDto>> Get(short id)
        {
            return await _permissionService.GetPermissionForViewAsync(id);
        }

        [HttpGet]
        [AppAuthorize(Permissions.IdentityPermissionGetList)]
        public async Task<Result<List<GetPermissionForViewDto>>> GetList(PermissionFilterDto filterDto)
        {
            return await _permissionService.GetListAsync(filterDto);
        }

        [HttpPost]
        [AppAuthorize(Permissions.IdentityPermissionCreateOrUpdate)]
        public async Task<Result<PermissionDto>> CreateOrUpdate([FromBody] CreateOrUpdatePermissionDto dto)
        {
            return await _permissionService.CreateOrUpdateAsync(dto);
        }

        [HttpGet]
        [Route("{id}")]
        [AppAuthorize(Permissions.IdentityPermissionDelete)]
        public async Task<Result> Delete(short id)
        {
            return await _permissionService.DeleteAsync(id);
        }
    }
}