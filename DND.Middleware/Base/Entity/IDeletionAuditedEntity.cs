using System;
using DND.Middleware.Entities.Identity;

namespace DND.Middleware.Base.Entity
{
    public interface IDeletionAuditedEntity
    {
        public bool IsDeleted { get; set; }
        int? DeleterUserId { get; set; }
        DateTime? DeletionTime { get; set; }
        User DeleterUser { get; set; }
    }
}
