using System;
using System.Linq;
using DND.Middleware.Constants;
using DND.Middleware.Entities.Identity;
using DND.Middleware.System.Options;
using Microsoft.AspNetCore.Identity;

namespace DND.Storage.Initializers
{
    public class IdentitySeeder
    {
        private readonly DateTime _now = DateTime.UtcNow;

        private readonly DatabaseContext _dbContext;
        private readonly AdministrationOptions _administrationOptions;

        private User _adminUser;
        private Role _adminRole;
        private Role _memberRole;

        public IdentitySeeder(DatabaseContext dbContext, AdministrationOptions administrationOptions)
        {
            _dbContext = dbContext;
            _administrationOptions = administrationOptions;
        }

        public void Seed()
        {
            SeedUsers();

            SeedRoles();

            SeedUserRoles();

            SeedPermissions();
        }

        private void SeedUsers()
        {
            _adminUser = _dbContext.Users.FirstOrDefault(x => x.UserName == Entities.Users.AdminUserName);
            if (_adminUser == null)
            {
                _adminUser = new User
                {
                    FirstName = Entities.Users.AdminFirstName,
                    LastName = Entities.Users.AdminLastName,
                    UserName = Entities.Users.AdminUserName,
                    Email = Entities.Users.AdminEmail,
                    PhoneNumber = Entities.Users.AdminPhoneNumber,
                    IsEmailConfirmed = true,
                    IsPhoneNumberConfirmed = true,
                    IsEmailEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    CreationTime = _now
                };
                _adminUser.PasswordHash = new PasswordHasher<User>().HashPassword(_adminUser, _administrationOptions.AdminPassword);
                _dbContext.Users.Add(_adminUser);
                _dbContext.SaveChanges();
            }
        }

        private void SeedRoles()
        {
            _adminRole = SeedRole(Entities.Roles.Admin);
            _memberRole = SeedRole(Entities.Roles.Member);
        }

        private Role SeedRole(string roleName)
        {
            var role = _dbContext.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
            {
                role = new Role { Name = roleName, CreationTime = _now, CreatorUserId = _adminUser.Id };
                _dbContext.Roles.Add(role);
                _dbContext.SaveChanges();
            }

            return role;
        }

        private void SeedUserRoles()
        {
            SeedUserRole(_adminUser.Id, _adminRole.Id);
            SeedUserRole(_adminUser.Id, _memberRole.Id);
        }

        private void SeedUserRole(int userId, short roleId)
        {
            var userRole = _dbContext.UserRoles.FirstOrDefault(x => x.UserId == userId && x.RoleId == roleId);
            if (userRole == null)
            {
                userRole = new UserRole { UserId = userId, RoleId = roleId, CreationTime = _now, CreatorUserId = _adminUser.Id };
                _dbContext.UserRoles.Add(userRole);
                _dbContext.SaveChanges();
            }
        }

        private void SeedPermissions()
        {
            var identityAccountChangePassword = SeedPermission(Permissions.IdentityAccountChangePassword);

            var identityUserGet = SeedPermission(Permissions.IdentityUserGet);
            var identityUserGetList = SeedPermission(Permissions.IdentityUserGetList);
            var identityUserCreate = SeedPermission(Permissions.IdentityUserCreateOrUpdate);
            var identityUserDelete = SeedPermission(Permissions.IdentityUserDelete);

            var identityRoleGet = SeedPermission(Permissions.IdentityRoleGet);
            var identityRoleGetList = SeedPermission(Permissions.IdentityRoleGetList);
            var identityRoleCreate = SeedPermission(Permissions.IdentityRoleCreateOrUpdate);
            var identityRoleDelete = SeedPermission(Permissions.IdentityRoleDelete);

            var identityUserRoleGet = SeedPermission(Permissions.IdentityUserRoleGet);
            var identityUserRoleGetList = SeedPermission(Permissions.IdentityUserRoleGetList);
            var identityUserRoleCreate = SeedPermission(Permissions.IdentityUserRoleCreateOrUpdate);
            var identityUserRoleDelete = SeedPermission(Permissions.IdentityUserRoleDelete);

            var identityPermissionGet = SeedPermission(Permissions.IdentityPermissionGet);
            var identityPermissionGetList = SeedPermission(Permissions.IdentityPermissionGetList);
            var identityPermissionCreate = SeedPermission(Permissions.IdentityPermissionCreateOrUpdate);
            var identityPermissionDelete = SeedPermission(Permissions.IdentityPermissionDelete);

            var identityRolePermissionGet = SeedPermission(Permissions.IdentityRolePermissionGet);
            var identityRolePermissionGetList = SeedPermission(Permissions.IdentityRolePermissionGetList);
            var identityRolePermissionCreate = SeedPermission(Permissions.IdentityRolePermissionCreateOrUpdate);
            var identityRolePermissionDelete = SeedPermission(Permissions.IdentityRolePermissionDelete);

            SeedRolePermission(_adminRole.Id, identityAccountChangePassword.Id);

            SeedRolePermission(_adminRole.Id, identityUserCreate.Id);
            SeedRolePermission(_adminRole.Id, identityUserDelete.Id);
            SeedRolePermission(_adminRole.Id, identityUserGet.Id);
            SeedRolePermission(_adminRole.Id, identityUserGetList.Id);

            SeedRolePermission(_adminRole.Id, identityRoleCreate.Id);
            SeedRolePermission(_adminRole.Id, identityRoleDelete.Id);
            SeedRolePermission(_adminRole.Id, identityRoleGet.Id);
            SeedRolePermission(_adminRole.Id, identityRoleGetList.Id);

            SeedRolePermission(_adminRole.Id, identityUserRoleCreate.Id);
            SeedRolePermission(_adminRole.Id, identityUserRoleDelete.Id);
            SeedRolePermission(_adminRole.Id, identityUserRoleGet.Id);
            SeedRolePermission(_adminRole.Id, identityUserRoleGetList.Id);

            SeedRolePermission(_adminRole.Id, identityPermissionCreate.Id);
            SeedRolePermission(_adminRole.Id, identityPermissionDelete.Id);
            SeedRolePermission(_adminRole.Id, identityPermissionGet.Id);
            SeedRolePermission(_adminRole.Id, identityPermissionGetList.Id);

            SeedRolePermission(_adminRole.Id, identityRolePermissionCreate.Id);
            SeedRolePermission(_adminRole.Id, identityRolePermissionDelete.Id);
            SeedRolePermission(_adminRole.Id, identityRolePermissionGet.Id);
            SeedRolePermission(_adminRole.Id, identityRolePermissionGetList.Id);

            SeedRolePermission(_memberRole.Id, identityAccountChangePassword.Id);
        }

        private Permission SeedPermission(string permissionName)
        {
            var permission = _dbContext.Permissions.FirstOrDefault(x => x.Name == permissionName);
            if (permission == null)
            {
                permission = new Permission { Name = permissionName, CreationTime = _now, CreatorUserId = _adminUser.Id };
                _dbContext.Permissions.Add(permission);
                _dbContext.SaveChanges();
            }

            return permission;
        }

        private void SeedRolePermission(short roleId, short permissionId)
        {
            var rolePermission = _dbContext.RolePermissions.FirstOrDefault(x => x.Id == roleId && x.PermissionId == permissionId);
            if (rolePermission == null)
            {
                rolePermission = new RolePermission { RoleId = roleId, PermissionId = permissionId, CreationTime = _now, CreatorUserId = _adminUser.Id };
                _dbContext.RolePermissions.Add(rolePermission);
                _dbContext.SaveChanges();
            }
        }
    }
}
