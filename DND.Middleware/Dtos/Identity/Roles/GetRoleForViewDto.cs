using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dtos.Identity.Roles
{
    public class GetRoleForViewDto : BaseViewDto
    {
        public RoleDto Role { get; set; }
    }
}