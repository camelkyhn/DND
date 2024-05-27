using System;
using DND.Middleware.Entity.Identity;

namespace DND.Middleware.Base
{
    public interface IAuditedEntity<TKey, TUserKey> : IEntity<TKey>
    {
        TUserKey CreatorUserId { get; set; }
        DateTimeOffset CreationTime { get; set; }
        TUserKey? ModifierUserId { get; set; }
        DateTimeOffset? ModificationTime { get; set; }
        TUserKey? DeletorUserId { get; set; }
        DateTimeOffset? DeletionTime { get; set; }
    }

    public class AuditedEntity<TKey, TUserKey> : Entity<TKey>, IAuditedEntity<TKey, TUserKey>
    {
        public TUserKey CreatorUserId { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public TUserKey? ModifierUserId { get; set; }
        public DateTimeOffset? ModificationTime { get; set; }
        public TUserKey? DeletorUserId { get; set; }
        public DateTimeOffset? DeletionTime { get; set; }

        public User CreatorUser { get; set; }
        public User ModifierUser { get; set; }
        public User DeletorUser { get; set; }
    }
}
