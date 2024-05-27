using DND.Middleware.Base;
using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Filter.Identity
{
    public class RolePermissionFilterDto : FilterDto
    {
        public short? RoleId { get; set; }

        [StringLength(MaxLengths.LongText)]
        public string RoleName { get; set; }

        public short? PermissionId { get; set; }

        [StringLength(MaxLengths.LongText)]
        public string PermissionName { get; set; }
    }
}
