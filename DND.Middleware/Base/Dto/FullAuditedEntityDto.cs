using System;

namespace DND.Middleware.Base.Dto
{
    public class FullAuditedEntityDto<TKey> : AuditedEntityDto<TKey>, IFullAuditedEntityDto
    {
        public int? DeleterUserId { get; set; }
        public DateTimeOffset? DeletionTime { get; set; }
    }
}
