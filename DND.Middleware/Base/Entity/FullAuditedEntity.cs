using DND.Middleware.Entity.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DND.Middleware.Base.Entity
{
    public class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IFullAuditedEntity
    {
        public int? DeleterUserId { get; set; }
        public DateTimeOffset? DeletionTime { get; set; }

        [ForeignKey(nameof(DeleterUserId))]
        public User DeleterUser { get; set; }
    }
}
