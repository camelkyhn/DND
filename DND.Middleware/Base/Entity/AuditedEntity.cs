using DND.Middleware.Entity.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DND.Middleware.Base.Entity
{
    public class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IAuditedEntity
    {
        public int? ModifierUserId { get; set; }
        public DateTimeOffset? ModificationTime { get; set; }

        [ForeignKey(nameof(ModifierUserId))]
        public User ModifierUser { get; set; }
    }
}
