using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Dto.Identity
{
    public class ForgotPasswordDto
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        public string Email { get; set; }
    }
}
