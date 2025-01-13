using System;

namespace DND.Middleware.Base.Dto
{
    public class FullAuditedEntityDto<TKey> : AuditedEntityDto<TKey>, IFullAuditedEntityDto
    {
        public bool IsDeleted { get; set; }
        public int? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
