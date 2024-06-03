using DND.Middleware.Base.Entity;
using DND.Middleware.Entity.Identity;
using System;
using System.Collections.Generic;

namespace DND.Middleware.System
{
    public static class EntityConfiguration
    {
        public static List<Type> TrackableList =>
        [
            typeof(User)
        ];

        public static List<string> IgnoredProperties =>
        [
            nameof(FullAuditedEntity<object>.CreationTime),
            nameof(FullAuditedEntity<object>.CreatorUser),
            nameof(FullAuditedEntity<object>.CreatorUserId),
            nameof(FullAuditedEntity<object>.ModificationTime),
            nameof(FullAuditedEntity<object>.ModifierUser),
            nameof(FullAuditedEntity<object>.ModifierUserId),
            nameof(FullAuditedEntity<object>.DeletionTime),
            nameof(FullAuditedEntity<object>.DeleterUser),
            nameof(FullAuditedEntity<object>.DeleterUserId),
            nameof(User.PasswordHash),
            nameof(User.SecurityStamp)
        ];
    }
}
