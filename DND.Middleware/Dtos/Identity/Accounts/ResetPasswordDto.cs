using System.ComponentModel.DataAnnotations;
using DND.Middleware.Constants;

namespace DND.Middleware.Dtos.Identity.Accounts
{
    public class ResetPasswordDto
    {
        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        public string Password { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        [Compare(nameof(Password), ErrorMessage = Errors.ConfirmPassword)]
        public string ConfirmPassword { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
