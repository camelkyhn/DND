using DND.Middleware.Base;
using DND.Middleware.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DND.Middleware.Entity.Identity
{
    [Table(nameof(Permission))]
    public class Permission : AuditedEntity<short, long>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        public string Name { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
