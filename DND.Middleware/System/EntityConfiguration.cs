using DND.Middleware.Base.Entity;
using DND.Middleware.Entities.Identity;
using System.Collections.Generic;

namespace DND.Middleware.System
{
    public static class EntityConfiguration
    {
        private static List<string> FullAuditedEntityIgnoredProperties =>
        [
            nameof(IFullAuditedEntity.CreationTime),
            nameof(IFullAuditedEntity.CreatorUser),
            nameof(IFullAuditedEntity.CreatorUserId),
            nameof(IFullAuditedEntity.LastModificationTime),
            nameof(IFullAuditedEntity.LastModifierUser),
            nameof(IFullAuditedEntity.LastModifierUserId),
            nameof(IFullAuditedEntity.DeletionTime),
            nameof(IFullAuditedEntity.DeleterUser),
            nameof(IFullAuditedEntity.DeleterUserId)
        ];

        public static List<TrackableEntity> GetTrackableEntityList()
        {
            var userIgnoredProperties = new List<string>
            {
                nameof(User.PasswordHash),
                nameof(User.SecurityStamp)
            };
            userIgnoredProperties.AddRange(FullAuditedEntityIgnoredProperties);
            return new List<TrackableEntity>
            {
                new()
                {
                    EntityType = typeof(User),
                    IgnoredProperties = userIgnoredProperties
                }
            };
        }
    }
}