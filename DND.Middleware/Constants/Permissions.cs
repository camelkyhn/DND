namespace DND.Middleware.Constants
{
    public class Permissions
    {
        #region Account

        public const string IdentityAccountChangePassword = $"{Areas.Identity}.{Controllers.Account}.{Actions.ChangePassword}";

        #endregion

        #region User

        public const string IdentityUserGet = $"{Areas.Identity}.{Controllers.User}.{Actions.Get}";
        public const string IdentityUserGetList = $"{Areas.Identity}.{Controllers.User}.{Actions.GetList}";
        public const string IdentityUserCreateOrUpdate = $"{Areas.Identity}.{Controllers.User}.{Actions.CreateOrUpdate}";
        public const string IdentityUserDelete = $"{Areas.Identity}.{Controllers.User}.{Actions.Delete}";

        #endregion

        #region Role

        public const string IdentityRoleGet = $"{Areas.Identity}.{Controllers.Role}.{Actions.Get}";
        public const string IdentityRoleGetList = $"{Areas.Identity}.{Controllers.Role}.{Actions.GetList}";
        public const string IdentityRoleCreateOrUpdate = $"{Areas.Identity}.{Controllers.Role}.{Actions.CreateOrUpdate}";
        public const string IdentityRoleDelete = $"{Areas.Identity}.{Controllers.Role}.{Actions.Delete}";

        #endregion

        #region UserRole

        public const string IdentityUserRoleGet = $"{Areas.Identity}.{Controllers.UserRole}.{Actions.Get}";
        public const string IdentityUserRoleGetList = $"{Areas.Identity}.{Controllers.UserRole}.{Actions.GetList}";
        public const string IdentityUserRoleCreateOrUpdate = $"{Areas.Identity}.{Controllers.UserRole}.{Actions.CreateOrUpdate}";
        public const string IdentityUserRoleDelete = $"{Areas.Identity}.{Controllers.UserRole}.{Actions.Delete}";

        #endregion

        #region Permission

        public const string IdentityPermissionGet = $"{Areas.Identity}.{Controllers.Permission}.{Actions.Get}";
        public const string IdentityPermissionGetList = $"{Areas.Identity}.{Controllers.Permission}.{Actions.GetList}";
        public const string IdentityPermissionCreateOrUpdate = $"{Areas.Identity}.{Controllers.Permission}.{Actions.CreateOrUpdate}";
        public const string IdentityPermissionDelete = $"{Areas.Identity}.{Controllers.Permission}.{Actions.Delete}";

        #endregion

        #region RolePermission

        public const string IdentityRolePermissionGet = $"{Areas.Identity}.{Controllers.RolePermission}.{Actions.Get}";
        public const string IdentityRolePermissionGetList = $"{Areas.Identity}.{Controllers.RolePermission}.{Actions.GetList}";
        public const string IdentityRolePermissionCreateOrUpdate = $"{Areas.Identity}.{Controllers.RolePermission}.{Actions.CreateOrUpdate}";
        public const string IdentityRolePermissionDelete = $"{Areas.Identity}.{Controllers.RolePermission}.{Actions.Delete}";

        #endregion
    }
}
