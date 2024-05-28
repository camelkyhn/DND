using AutoMapper;
using DND.Storage.IRepositories.Identity;
using DND.Storage.Repositories.Identity;
using System;

namespace DND.Storage
{
    public class RepositoryContext : IRepositoryContext
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _databaseContext;

        private IPermissionRepository _permissionRepository;
        private IRoleRepository _roleRepository;
        private IRolePermissionRepository _rolePermissionRepository;
        private IUserRepository _userRepository;
        private IUserRoleRepository _userRoleRepository;

        public RepositoryContext(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public IPermissionRepository Permissions { get { return _permissionRepository ??= new PermissionRepository(_databaseContext, _mapper); } }
        public IRoleRepository Roles { get { return _roleRepository ??= new RoleRepository(_databaseContext, _mapper); } }
        public IRolePermissionRepository RolePermissions { get { return _rolePermissionRepository ??= new RolePermissionRepository(_databaseContext, _mapper); } }
        public IUserRepository Users { get { return _userRepository ??= new UserRepository(_databaseContext, _mapper); } }
        public IUserRoleRepository UserRoles { get { return _userRoleRepository ??= new UserRoleRepository(_databaseContext, _mapper); } }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _databaseContext.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
