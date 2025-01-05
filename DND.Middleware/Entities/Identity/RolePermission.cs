using DND.Middleware.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DND.Middleware.Entities.Identity
{
    [Table(nameof(RolePermission))]
    public class RolePermission : FullAuditedEntity<int>
    {
        public short RoleId { get; set; }
        public short PermissionId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        [ForeignKey(nameof(PermissionId))]
        public Permission Permission { get; set; }
    }
}
