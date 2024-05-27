namespace DND.Middleware.Constants
{
    public class Permissions
    {
        #region Account

        public const string IdentityAccountAccessDenied = $"{Areas.Identity}.{Controllers.Account}.{Actions.AccessDenied}";
        public const string IdentityAccountChangePassword = $"{Areas.Identity}.{Controllers.Account}.{Actions.ChangePassword}";
        public const string IdentityAccountConfirmEmail = $"{Areas.Identity}.{Controllers.Account}.{Actions.ConfirmEmail}";
        public const string IdentityAccountLogin = $"{Areas.Identity}.{Controllers.Account}.{Actions.Login}";
        public const string IdentityAccountLogout = $"{Areas.Identity}.{Controllers.Account}.{Actions.Logout}";
        public const string IdentityAccountResetPassword = $"{Areas.Identity}.{Controllers.Account}.{Actions.ResetPassword}";

        #endregion

        #region User

        public const string IdentityUserCreate = $"{Areas.Identity}.{Controllers.User}.{Actions.Create}";
        public const string IdentityUserDelete = $"{Areas.Identity}.{Controllers.User}.{Actions.Delete}";
        public const string IdentityUserDetail = $"{Areas.Identity}.{Controllers.User}.{Actions.Detail}";
        public const string IdentityUserEdit = $"{Areas.Identity}.{Controllers.User}.{Actions.Edit}";
        public const string IdentityUserList = $"{Areas.Identity}.{Controllers.User}.{Actions.List}";

        #endregion

        #region Role

        public const string IdentityRoleCreate = $"{Areas.Identity}.{Controllers.Role}.{Actions.Create}";
        public const string IdentityRoleDelete = $"{Areas.Identity}.{Controllers.Role}.{Actions.Delete}";
        public const string IdentityRoleDetail = $"{Areas.Identity}.{Controllers.Role}.{Actions.Detail}";
        public const string IdentityRoleEdit = $"{Areas.Identity}.{Controllers.Role}.{Actions.Edit}";
        public const string IdentityRoleList = $"{Areas.Identity}.{Controllers.Role}.{Actions.List}";

        #endregion

        #region UserRole

        public const string IdentityUserRoleCreate = $"{Areas.Identity}.{Controllers.UserRole}.{Actions.Create}";
        public const string IdentityUserRoleDelete = $"{Areas.Identity}.{Controllers.UserRole}.{Actions.Delete}";
        public const string IdentityUserRoleDetail = $"{Areas.Identity}.{Controllers.UserRole}.{Actions.Detail}";
        public const string IdentityUserRoleEdit = $"{Areas.Identity}.{Controllers.UserRole}.{Actions.Edit}";
        public const string IdentityUserRoleList = $"{Areas.Identity}.{Controllers.UserRole}.{Actions.List}";

        #endregion

        #region Permission

        public const string IdentityPermissionCreate = $"{Areas.Identity}.{Controllers.Permission}.{Actions.Create}";
        public const string IdentityPermissionDelete = $"{Areas.Identity}.{Controllers.Permission}.{Actions.Delete}";
        public const string IdentityPermissionDetail = $"{Areas.Identity}.{Controllers.Permission}.{Actions.Detail}";
        public const string IdentityPermissionEdit = $"{Areas.Identity}.{Controllers.Permission}.{Actions.Edit}";
        public const string IdentityPermissionList = $"{Areas.Identity}.{Controllers.Permission}.{Actions.List}";

        #endregion

        #region RolePermission

        public const string IdentityRolePermissionCreate = $"{Areas.Identity}.{Controllers.RolePermission}.{Actions.Create}";
        public const string IdentityRolePermissionDelete = $"{Areas.Identity}.{Controllers.RolePermission}.{Actions.Delete}";
        public const string IdentityRolePermissionDetail = $"{Areas.Identity}.{Controllers.RolePermission}.{Actions.Detail}";
        public const string IdentityRolePermissionEdit = $"{Areas.Identity}.{Controllers.RolePermission}.{Actions.Edit}";
        public const string IdentityRolePermissionList = $"{Areas.Identity}.{Controllers.RolePermission}.{Actions.List}";

        #endregion
    }
}
