using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dto.Identity
{
    public class UserRoleDto : AuditedEntityDto<long?>
    {
        public int UserId { get; set; }
        public short RoleId { get; set; }
    }
}
