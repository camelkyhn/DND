using DND.Middleware.Base.Dto;
using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Dto.Identity
{
    public class PermissionDto : AuditedEntityDto<short?>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        public string Name { get; set; }
    }
}
