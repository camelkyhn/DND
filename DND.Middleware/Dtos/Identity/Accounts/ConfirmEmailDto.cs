using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Dtos.Identity.Accounts
{
    public class ConfirmEmailDto
    {
        public int UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
