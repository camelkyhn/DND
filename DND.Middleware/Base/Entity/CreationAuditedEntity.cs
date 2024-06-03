using DND.Middleware.Entity.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DND.Middleware.Base.Entity
{
    public class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAuditedEntity
    {
        public int CreatorUserId { get; set; }
        public DateTimeOffset CreationTime { get; set; }

        [ForeignKey(nameof(CreatorUserId))]
        public User CreatorUser { get; set; }
    }
}
