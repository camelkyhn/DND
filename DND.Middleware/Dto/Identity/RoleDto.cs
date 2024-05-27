using DND.Middleware.Base;
using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Dto.Identity
{
    public class RoleDto : AuditedEntityDto<short?, long>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        public string Name { get; set; }
    }
}
