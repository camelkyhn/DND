using DND.Middleware.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace DND.Middleware.Entity.Identity
{
    [Table(nameof(UserRole))]
    public class UserRole : AuditedEntity<int, long>
    {
        public long UserId { get; set; }
        public short RoleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
