using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dto.Identity
{
    public class RolePermissionDto : AuditedEntityDto<int?>
    {
        public short RoleId { get; set; }
        public short PermissionId { get; set; }
    }
}
