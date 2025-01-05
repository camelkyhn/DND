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
            var identityAccountAccessDenied = SeedPermission(Permissions.IdentityAccountAccessDenied);
            var identityAccountChangePassword = SeedPermission(Permissions.IdentityAccountChangePassword);
            var identityAccountConfirmEmail = SeedPermission(Permissions.IdentityAccountConfirmEmail);
            var identityAccountLogin = SeedPermission(Permissions.IdentityAccountLogin);
            var identityAccountLogout = SeedPermission(Permissions.IdentityAccountLogout);
            var identityAccountResetPassword = SeedPermission(Permissions.IdentityAccountResetPassword);

            var identityUserCreate = SeedPermission(Permissions.IdentityUserCreate);
            var identityUserDelete = SeedPermission(Permissions.IdentityUserDelete);
            var identityUserDetail = SeedPermission(Permissions.IdentityUserDetail);
            var identityUserUpdate = SeedPermission(Permissions.IdentityUserUpdate);
            var identityUserList = SeedPermission(Permissions.IdentityUserList);

            var identityRoleCreate = SeedPermission(Permissions.IdentityRoleCreate);
            var identityRoleDelete = SeedPermission(Permissions.IdentityRoleDelete);
            var identityRoleDetail = SeedPermission(Permissions.IdentityRoleDetail);
            var identityRoleUpdate = SeedPermission(Permissions.IdentityRoleUpdate);
            var identityRoleList = SeedPermission(Permissions.IdentityRoleList);

            var identityUserRoleCreate = SeedPermission(Permissions.IdentityUserRoleCreate);
            var identityUserRoleDelete = SeedPermission(Permissions.IdentityUserRoleDelete);
            var identityUserRoleDetail = SeedPermission(Permissions.IdentityUserRoleDetail);
            var identityUserRoleUpdate = SeedPermission(Permissions.IdentityUserRoleUpdate);
            var identityUserRoleList = SeedPermission(Permissions.IdentityUserRoleList);

            var identityPermissionCreate = SeedPermission(Permissions.IdentityPermissionCreate);
            var identityPermissionDelete = SeedPermission(Permissions.IdentityPermissionDelete);
            var identityPermissionDetail = SeedPermission(Permissions.IdentityPermissionDetail);
            var identityPermissionUpdate = SeedPermission(Permissions.IdentityPermissionUpdate);
            var identityPermissionList = SeedPermission(Permissions.IdentityPermissionList);

            var identityRolePermissionCreate = SeedPermission(Permissions.IdentityRolePermissionCreate);
            var identityRolePermissionDelete = SeedPermission(Permissions.IdentityRolePermissionDelete);
            var identityRolePermissionDetail = SeedPermission(Permissions.IdentityRolePermissionDetail);
            var identityRolePermissionUpdate = SeedPermission(Permissions.IdentityRolePermissionUpdate);
            var identityRolePermissionList = SeedPermission(Permissions.IdentityRolePermissionList);

            SeedRolePermission(_adminRole.Id, identityAccountChangePassword.Id);

            SeedRolePermission(_adminRole.Id, identityUserCreate.Id);
            SeedRolePermission(_adminRole.Id, identityUserDelete.Id);
            SeedRolePermission(_adminRole.Id, identityUserDetail.Id);
            SeedRolePermission(_adminRole.Id, identityUserUpdate.Id);
            SeedRolePermission(_adminRole.Id, identityUserList.Id);

            SeedRolePermission(_adminRole.Id, identityRoleCreate.Id);
            SeedRolePermission(_adminRole.Id, identityRoleDelete.Id);
            SeedRolePermission(_adminRole.Id, identityRoleDetail.Id);
            SeedRolePermission(_adminRole.Id, identityRoleUpdate.Id);
            SeedRolePermission(_adminRole.Id, identityRoleList.Id);

            SeedRolePermission(_adminRole.Id, identityUserRoleCreate.Id);
            SeedRolePermission(_adminRole.Id, identityUserRoleDelete.Id);
            SeedRolePermission(_adminRole.Id, identityUserRoleDetail.Id);
            SeedRolePermission(_adminRole.Id, identityUserRoleUpdate.Id);
            SeedRolePermission(_adminRole.Id, identityUserRoleList.Id);

            SeedRolePermission(_adminRole.Id, identityPermissionCreate.Id);
            SeedRolePermission(_adminRole.Id, identityPermissionDelete.Id);
            SeedRolePermission(_adminRole.Id, identityPermissionDetail.Id);
            SeedRolePermission(_adminRole.Id, identityPermissionUpdate.Id);
            SeedRolePermission(_adminRole.Id, identityPermissionList.Id);

            SeedRolePermission(_adminRole.Id, identityRolePermissionCreate.Id);
            SeedRolePermission(_adminRole.Id, identityRolePermissionDelete.Id);
            SeedRolePermission(_adminRole.Id, identityRolePermissionDetail.Id);
            SeedRolePermission(_adminRole.Id, identityRolePermissionUpdate.Id);
            SeedRolePermission(_adminRole.Id, identityRolePermissionList.Id);

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
