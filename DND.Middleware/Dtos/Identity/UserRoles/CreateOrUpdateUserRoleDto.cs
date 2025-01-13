using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dtos.Identity.UserRoles
{
    public class CreateOrUpdateUserRoleDto : EntityDto<long?>
    {
        public int UserId { get; set; }
        public short RoleId { get; set; }
    }
}