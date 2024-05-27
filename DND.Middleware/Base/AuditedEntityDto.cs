using System;

namespace DND.Middleware.Base
{
    public interface IAuditedEntityDto<TKey, TUserKey> : IEntityDto<TKey>
    {
        TUserKey CreatorUserId { get; set; }
        DateTimeOffset CreationTime { get; set; }
        TUserKey? ModifierUserId { get; set; }
        DateTimeOffset? ModificationTime { get; set; }
        TUserKey? DeletorUserId { get; set; }
        DateTimeOffset? DeletionTime { get; set; }
    }

    public class AuditedEntityDto<TKey, TUserKey> : EntityDto<TKey>, IAuditedEntityDto<TKey, TUserKey>
    {
        public TUserKey CreatorUserId { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public TUserKey? ModifierUserId { get; set; }
        public DateTimeOffset? ModificationTime { get; set; }
        public TUserKey? DeletorUserId { get; set; }
        public DateTimeOffset? DeletionTime { get; set; }
    }
}
