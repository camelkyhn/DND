using DND.Middleware.Entity.Identity;
using System;

namespace DND.Middleware.Base.Entity
{
    public class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IFullAuditedEntity
    {
        public int? DeleterUserId { get; set; }
        public DateTimeOffset? DeletionTime { get; set; }

        public User DeleterUser { get; set; }
    }
}
