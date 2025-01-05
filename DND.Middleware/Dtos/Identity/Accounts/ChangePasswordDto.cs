using System.ComponentModel.DataAnnotations;
using DND.Middleware.Constants;

namespace DND.Middleware.Dtos.Identity.Accounts
{
    public class ChangePasswordDto
    {
        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        public string ConfirmPassword { get; set; }
    }
}
