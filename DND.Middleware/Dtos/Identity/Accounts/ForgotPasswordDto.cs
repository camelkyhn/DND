using System.ComponentModel.DataAnnotations;
using DND.Middleware.Constants;

namespace DND.Middleware.Dtos.Identity.Accounts
{
    public class ForgotPasswordDto
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        public string Email { get; set; }
    }
}
