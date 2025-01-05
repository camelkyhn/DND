using System;

namespace DND.Middleware.Base.Dto
{
    public class AuditedEntityDto<TKey> : CreationAuditedEntityDto<TKey>, IAuditedEntityDto
    {
        public int? ModifierUserId { get; set; }
        public DateTime? ModificationTime { get; set; }
    }
}
