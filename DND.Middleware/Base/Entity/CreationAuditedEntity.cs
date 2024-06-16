using DND.Middleware.Entity.Identity;
using System;

namespace DND.Middleware.Base.Entity
{
    public class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAuditedEntity
    {
        public int CreatorUserId { get; set; }
        public DateTimeOffset CreationTime { get; set; }

        public User CreatorUser { get; set; }
    }
}
