using DND.Middleware.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DND.Middleware.Entities.Identity
{
    [Table(nameof(UserRole))]
    public class UserRole : FullAuditedEntity<long>
    {
        public int UserId { get; set; }
        public short RoleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
