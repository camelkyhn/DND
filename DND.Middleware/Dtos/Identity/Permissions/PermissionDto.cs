using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dtos.Identity.Permissions
{
    public class PermissionDto : EntityDto<short>
    {
        public string Name { get; set; }
    }
}
