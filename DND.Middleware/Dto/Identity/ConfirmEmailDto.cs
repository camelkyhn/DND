using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Dto.Identity
{
    public class ConfirmEmailDto
    {
        public long UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
