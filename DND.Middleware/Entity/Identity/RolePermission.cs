using DND.Middleware.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace DND.Middleware.Entity.Identity
{
    [Table(nameof(RolePermission))]
    public class RolePermission : AuditedEntity<int, long>
    {
        public short RoleId { get; set; }
        public short PermissionId { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
