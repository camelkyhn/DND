using System;

namespace DND.Middleware.Base.Dto
{
    public interface IDeletionAuditedEntityDto
    {
        int? DeleterUserId { get; set; }
        DateTime? DeletionTime { get; set; }
    }
}
