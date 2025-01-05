using DND.Middleware.Base.Entity;
using DND.Middleware.Constants;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DND.Middleware.Entities.Identity
{
    [Table(nameof(Role))]
    [Index(nameof(Name), IsUnique = true)]
    public class Role : FullAuditedEntity<short>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
