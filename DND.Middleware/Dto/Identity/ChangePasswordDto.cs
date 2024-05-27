using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Dto.Identity
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
