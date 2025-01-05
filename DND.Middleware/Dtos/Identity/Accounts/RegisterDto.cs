using System.ComponentModel.DataAnnotations;
using DND.Middleware.Constants;

namespace DND.Middleware.Dtos.Identity.Accounts
{
    public class RegisterDto
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        public string Email { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        public string Password { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        public string ConfirmPassword { get; set; }

        [StringLength(MaxLengths.TinyText)]
        public string PhoneNumber { get; set; }
    }
}
