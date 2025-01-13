using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dtos.Identity.RolePermissions
{
    public class GetRolePermissionForViewDto : BaseViewDto
    {
        public RolePermissionDto RolePermission { get; set; }
        public string RoleName { get; set; }
        public string PermissionName { get; set; }
    }
}