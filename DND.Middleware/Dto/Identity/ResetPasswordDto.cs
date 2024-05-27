using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Dto.Identity
{
    public class ResetPasswordDto
    {
        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        public string Password { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        public string ConfirmPassword { get; set; }

        public long UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
