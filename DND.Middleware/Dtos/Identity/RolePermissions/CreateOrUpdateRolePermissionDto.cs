﻿using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dtos.Identity.RolePermissions
{
    public class CreateOrUpdateRolePermissionDto : EntityDto<int?>
    {
        public short RoleId { get; set; }
        public short PermissionId { get; set; }
    }
}