using DND.Middleware.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace DND.Storage.Extensions
{
    public static class IdentityRelations
    {
        public static void AddAuditedUserRelations(this ModelBuilder builder)
        {
            builder.Entity<User>(user =>
            {
                #region Created User

                user.HasMany(u => u.CreatedPermissions)
                    .WithOne(x => x.CreatorUser)
                    .HasForeignKey(x => x.CreatorUserId)
                    .IsRequired(false);

                user.HasMany(u => u.CreatedRolePermissions)
                    .WithOne(x => x.CreatorUser)
                    .HasForeignKey(x => x.CreatorUserId)
                    .IsRequired(false);

                user.HasMany(u => u.CreatedRoles)
                    .WithOne(x => x.CreatorUser)
                    .HasForeignKey(x => x.CreatorUserId)
                    .IsRequired(false);

                user.HasMany(u => u.CreatedUserRoles)
                    .WithOne(x => x.CreatorUser)
                    .HasForeignKey(x => x.CreatorUserId)
                    .IsRequired(false);

                user.HasMany(u => u.CreatedUsers)
                    .WithOne(x => x.CreatorUser)
                    .HasForeignKey(x => x.CreatorUserId)
                    .IsRequired(false);

                #endregion

                #region Modified User

                user.HasMany(u => u.ModifiedPermissions)
                    .WithOne(x => x.ModifierUser)
                    .HasForeignKey(x => x.ModifierUserId)
                    .IsRequired(false);

                user.HasMany(u => u.ModifiedRolePermissions)
                    .WithOne(x => x.ModifierUser)
                    .HasForeignKey(x => x.ModifierUserId)
                    .IsRequired(false);

                user.HasMany(u => u.ModifiedRoles)
                    .WithOne(x => x.ModifierUser)
                    .HasForeignKey(x => x.ModifierUserId)
                    .IsRequired(false);

                user.HasMany(u => u.ModifiedUserRoles)
                    .WithOne(x => x.ModifierUser)
                    .HasForeignKey(x => x.ModifierUserId)
                    .IsRequired(false);

                user.HasMany(u => u.ModifiedUsers)
                    .WithOne(x => x.ModifierUser)
                    .HasForeignKey(x => x.ModifierUserId)
                    .IsRequired(false);

                #endregion

                #region Deleted User

                user.HasMany(u => u.DeletedPermissions)
                    .WithOne(x => x.DeleterUser)
                    .HasForeignKey(x => x.DeleterUserId)
                    .IsRequired(false);

                user.HasMany(u => u.DeletedRolePermissions)
                    .WithOne(x => x.DeleterUser)
                    .HasForeignKey(x => x.DeleterUserId)
                    .IsRequired(false);

                user.HasMany(u => u.DeletedRoles)
                    .WithOne(x => x.DeleterUser)
                    .HasForeignKey(x => x.DeleterUserId)
                    .IsRequired(false);

                user.HasMany(u => u.DeletedUserRoles)
                    .WithOne(x => x.DeleterUser)
                    .HasForeignKey(x => x.DeleterUserId)
                    .IsRequired(false);

                user.HasMany(u => u.DeletedUsers)
                    .WithOne(x => x.DeleterUser)
                    .HasForeignKey(x => x.DeleterUserId)
                    .IsRequired(false);

                #endregion
            });
        }
    }
}
