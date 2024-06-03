using System;

namespace DND.Middleware.Base.Dto
{
    public interface IDeletionAuditedEntityDto
    {
        int? DeleterUserId { get; set; }
        DateTimeOffset? DeletionTime { get; set; }
    }
}
