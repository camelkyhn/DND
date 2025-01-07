using DND.Middleware.Entities.Identity;
using System;

namespace DND.Middleware.Base.Entity
{
    public class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IAuditedEntity
    {
        public int? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public User LastModifierUser { get; set; }
    }
}
