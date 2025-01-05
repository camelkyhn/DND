using System;
using System.ComponentModel.DataAnnotations;
using DND.Middleware.Base.Dto;
using DND.Middleware.Constants;

namespace DND.Middleware.Dtos.Identity.Users;

public class CreateOrUpdateUserDto : EntityDto<int?>
{
    [Required]
    [StringLength(MaxLengths.TinyText, MinimumLength = MinLengths.TinyText)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(MaxLengths.TinyText, MinimumLength = MinLengths.TinyText)]
    public string LastName { get; set; }

    [Required]
    [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
    public string UserName { get; set; }

    [Required]
    [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
    public string Email { get; set; }

    [StringLength(MaxLengths.TinyText)]
    public string PhoneNumber { get; set; }

    [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
    public string PasswordHash { get; set; }

    public bool IsEmailEnabled { get; set; }

    public bool IsSmsEnabled { get; set; }

    [StringLength(MaxLengths.GuidText, MinimumLength = MinLengths.GuidText)]
    public string SecurityStamp { get; set; }

    public bool IsEmailConfirmed { get; set; }

    public bool IsPhoneNumberConfirmed { get; set; }

    public bool IsTwoFactorEnabled { get; set; }

    public short AccessFailedCount { get; set; }

    public bool IsLockoutEnabled { get; set; }

    public DateTime? LockoutEnd { get; set; }
}