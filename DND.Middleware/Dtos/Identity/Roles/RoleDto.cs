using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dtos.Identity.Roles
{
    public class RoleDto : EntityDto<short>
    {
        public string Name { get; set; }
    }
}
