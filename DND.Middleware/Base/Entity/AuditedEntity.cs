using DND.Middleware.Entities.Identity;
using System;

namespace DND.Middleware.Base.Entity
{
    public class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IAuditedEntity
    {
        public int? ModifierUserId { get; set; }
        public DateTime? ModificationTime { get; set; }

        public User ModifierUser { get; set; }
    }
}
