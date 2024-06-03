using DND.Middleware.Base.Entity;
using DND.Middleware.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DND.Middleware.Entity.Identity
{
    [Table(nameof(User))]
    [Index(nameof(Email), IsUnique = true)]
    public class User : FullAuditedEntity<int>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        public string Email { get; set; }

        [StringLength(MaxLengths.TinyText)]
        public string PhoneNumber { get; set; }

        public string PasswordHash { get; set; }

        public bool IsEmailEnabled { get; set; }

        public bool IsSmsEnabled { get; set; }

        public string SecurityStamp { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }

        public bool IsTwoFactorEnabled { get; set; }

        public short AccessFailedCount { get; set; }

        public bool IsLockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        #region User Created Relations

        public virtual ICollection<Permission> CreatedPermissions { get; set; }
        public virtual ICollection<User> CreatedUsers { get; set; }
        public virtual ICollection<Role> CreatedRoles { get; set; }
        public virtual ICollection<RolePermission> CreatedRolePermissions { get; set; }
        public virtual ICollection<UserRole> CreatedUserRoles { get; set; }

        #endregion

        #region User Modified Relations

        public virtual ICollection<Permission> ModifiedPermissions { get; set; }
        public virtual ICollection<User> ModifiedUsers { get; set; }
        public virtual ICollection<Role> ModifiedRoles { get; set; }
        public virtual ICollection<RolePermission> ModifiedRolePermissions { get; set; }
        public virtual ICollection<UserRole> ModifiedUserRoles { get; set; }

        #endregion

        #region User Deleted Relations

        public virtual ICollection<Permission> DeletedPermissions { get; set; }
        public virtual ICollection<User> DeletedUsers { get; set; }
        public virtual ICollection<Role> DeletedRoles { get; set; }
        public virtual ICollection<RolePermission> DeletedRolePermissions { get; set; }
        public virtual ICollection<UserRole> DeletedUserRoles { get; set; }

        #endregion
    }
}
