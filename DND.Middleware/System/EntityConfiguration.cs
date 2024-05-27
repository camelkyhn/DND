using DND.Middleware.Base;
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
            nameof(AuditedEntity<object, object>.CreationTime),
            nameof(AuditedEntity<object, object>.CreatorUser),
            nameof(AuditedEntity<object, object>.CreatorUserId),
            nameof(AuditedEntity<object, object>.ModificationTime),
            nameof(AuditedEntity<object, object>.ModifierUser),
            nameof(AuditedEntity<object, object>.ModifierUserId),
            nameof(AuditedEntity<object, object>.DeletionTime),
            nameof(AuditedEntity<object, object>.DeletorUser),
            nameof(AuditedEntity<object, object>.DeletorUserId),
            nameof(User.PasswordHash),
            nameof(User.SecurityStamp)
        ];
    }
}
