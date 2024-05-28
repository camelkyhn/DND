using DND.Storage.IRepositories.Identity;
using System;

namespace DND.Storage
{
    public interface IRepositoryContext : IDisposable
    {
        public IPermissionRepository Permissions { get; }
        public IRoleRepository Roles { get; }
        public IRolePermissionRepository RolePermissions { get; }
        public IUserRepository Users { get; }
        public IUserRoleRepository UserRoles { get; }
    }
}
