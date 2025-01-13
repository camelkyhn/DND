using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dtos.Identity.UserRoles
{
    public class GetUserRoleForViewDto : BaseViewDto
    {
        public UserRoleDto UserRole { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string RoleName { get; set; }
    }
}