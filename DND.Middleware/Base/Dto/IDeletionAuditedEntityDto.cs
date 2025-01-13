using System;

namespace DND.Middleware.Base.Dto
{
    public interface IDeletionAuditedEntityDto
    {
        public bool IsDeleted { get; set; }
        int? DeleterUserId { get; set; }
        DateTime? DeletionTime { get; set; }
    }
}
