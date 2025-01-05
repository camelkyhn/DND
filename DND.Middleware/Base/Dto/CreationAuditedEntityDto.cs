using System;

namespace DND.Middleware.Base.Dto
{
    public class CreationAuditedEntityDto<TKey> : EntityDto<TKey>, ICreationAuditedEntityDto
    {
        public int CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
