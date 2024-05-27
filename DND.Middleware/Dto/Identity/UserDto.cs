using DND.Middleware.Base;
using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;
using System;

namespace DND.Middleware.Dto.Identity
{
    public class UserDto : AuditedEntityDto<long?, long>
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
    }
}
