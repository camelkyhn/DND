using DND.Middleware.Base;

namespace DND.Middleware.Dto.Identity
{
    public class UserRoleDto : AuditedEntityDto<int?, long>
    {
        public long UserId { get; set; }
        public short RoleId { get; set; }
    }
}
