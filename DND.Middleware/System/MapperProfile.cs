using DND.Middleware.Entities.Identity;
using AutoMapper;
using DND.Middleware.Dtos.Identity.Permissions;
using DND.Middleware.Dtos.Identity.RolePermissions;
using DND.Middleware.Dtos.Identity.Roles;
using DND.Middleware.Dtos.Identity.UserRoles;
using DND.Middleware.Dtos.Identity.Users;

namespace DND.Middleware.System
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Identity

            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<Permission, CreateOrUpdatePermissionDto>().ReverseMap();

            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, CreateOrUpdateRoleDto>().ReverseMap();

            CreateMap<RolePermission, RolePermissionDto>().ReverseMap();
            CreateMap<RolePermission, CreateOrUpdateRolePermissionDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateOrUpdateUserDto>().ReverseMap();

            CreateMap<UserRole, UserRoleDto>().ReverseMap();
            CreateMap<UserRole, CreateOrUpdateUserRoleDto>().ReverseMap();

            #endregion
        }
    }
}
