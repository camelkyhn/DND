using System;
using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dtos.Identity.Users
{
    public class UserDto : EntityDto<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

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

        public DateTime? LockoutEnd { get; set; }
    }
}
