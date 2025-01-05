using DND.Middleware.Base.Filter;
using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.FilterDtos.Identity
{
    public class UserFilterDto : FilterDto
    {
        [StringLength(MaxLengths.LongText)]
        public string Email { get; set; }

        public bool? IsEmailConfirmed { get; set; }

        public bool? IsEmailEnabled { get; set; }
    }
}
