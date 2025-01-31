﻿namespace DND.Middleware.Constants
{
    public class Cache
    {
        public class Users
        {
            public const string AdminKey = "User.Admin";
        }

        public class Roles
        {
            public const string AdminKey = "Role.Admin";
            public const string MemberKey = "Role.Member";
            public const string AdminValue = "Admin";
            public const string MemberValue = "Member";
        }

        public class Lists
        {
            public const string RolePermission = "List.RolePermission";
        }
    }
}