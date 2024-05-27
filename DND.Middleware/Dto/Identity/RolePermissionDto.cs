using DND.Middleware.Base;

namespace DND.Middleware.Dto.Identity
{
    public class RolePermissionDto : AuditedEntityDto<int?, long>
    {
        public short RoleId { get; set; }
        public short PermissionId { get; set; }
    }
}
