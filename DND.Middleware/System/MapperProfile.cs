using DND.Middleware.Dto.Identity;
using DND.Middleware.Entity.Identity;
using AutoMapper;

namespace DND.Middleware.System
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<RolePermission, RolePermissionDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRole, UserRoleDto>().ReverseMap();
        }
    }
}
