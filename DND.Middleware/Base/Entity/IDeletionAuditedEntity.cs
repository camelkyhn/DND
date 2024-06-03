using System;
using DND.Middleware.Entity.Identity;

namespace DND.Middleware.Base.Entity
{
    public interface IDeletionAuditedEntity
    {
        int? DeleterUserId { get; set; }
        DateTimeOffset? DeletionTime { get; set; }
        User DeleterUser { get; set; }
    }
}
