using System;
using DND.Middleware.Entities.Identity;

namespace DND.Middleware.Base.Entity
{
    public interface IDeletionAuditedEntity
    {
        int? DeleterUserId { get; set; }
        DateTime? DeletionTime { get; set; }
        User DeleterUser { get; set; }
    }
}
