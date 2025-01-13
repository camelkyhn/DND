using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dtos.Identity.Permissions
{
    public class GetPermissionForViewDto : BaseViewDto
    {
        public PermissionDto Permission { get; set; }
    }
}