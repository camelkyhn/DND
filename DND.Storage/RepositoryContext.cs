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

        public IPermissionRepository Permissions
        {
            get
            {
                if (_permissionRepository == null)
                {
                    _permissionRepository = new PermissionRepository(_databaseContext, _mapper);
                    return _permissionRepository;
                }
                else
                {
                    return _permissionRepository;
                }
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository = new RoleRepository(_databaseContext, _mapper);
                    return _roleRepository;
                }
                else
                {
                    return _roleRepository;
                }
            }
        }

        public IRolePermissionRepository RolePermissions
        {
            get
            {
                if (_rolePermissionRepository == null)
                {
                    _rolePermissionRepository = new RolePermissionRepository(_databaseContext, _mapper);
                    return _rolePermissionRepository;
                }
                else
                {
                    return _rolePermissionRepository;
                }
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_databaseContext, _mapper);
                    return _userRepository;
                }
                else
                {
                    return _userRepository;
                }
            }
        }

        public IUserRoleRepository UserRoles
        {
            get
            {
                if (_userRoleRepository == null)
                {
                    _userRoleRepository = new UserRoleRepository(_databaseContext, _mapper);
                    return _userRoleRepository;
                }
                else
                {
                    return _userRoleRepository;
                }
            }
        }

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
