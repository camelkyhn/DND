using DND.Middleware.Base.Dto;

namespace DND.Middleware.Dtos.Identity.Users
{
    public class GetUserForViewDto : BaseViewDto
    {
        public UserDto User { get; set; }
    }
}