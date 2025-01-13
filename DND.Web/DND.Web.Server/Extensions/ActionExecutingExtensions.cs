using System;
using DND.Middleware.Extensions;
using DND.Storage.Repositories.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DND.Web.Server.Extensions
{
    public static class ActionExecutingExtensions
    {
        public static bool HasAccess(this ActionExecutingContext context, string permission)
        {
            var userRoleRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRoleRepository>();
            var rolePermissionRepository = context.HttpContext.RequestServices.GetRequiredService<IRolePermissionRepository>();
            try
            {
                var roleIdList = userRoleRepository.GetUserRoleIdListAsync(context.HttpContext.User.GetUserId().GetValueOrDefault()).GetAwaiter().GetResult();
                var permissionList = rolePermissionRepository.GetRolePermissionListAsync(roleIdList).GetAwaiter().GetResult();
                return permissionList.Contains(permission);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}