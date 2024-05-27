using DND.Middleware.Base;
using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Filter.Identity
{
    public class UserFilterDto : FilterDto
    {
        [StringLength(MaxLengths.LongText)]
        public string Email { get; set; }

        public bool? IsEmailConfirmed { get; set; }

        public bool? IsEmailEnabled { get; set; }
    }
}
