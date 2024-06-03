using DND.Middleware.Base.Filter;
using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Filter.Identity
{
    public class UserRoleFilterDto : FilterDto
    {
        public int? UserId { get; set; }

        [StringLength(MaxLengths.LongText)]
        public string UserEmail { get; set; }

        [StringLength(MaxLengths.LongText)]
        public string RoleName { get; set; }
    }
}
