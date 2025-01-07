using DND.Middleware.Entities.Identity;
using System;

namespace DND.Middleware.Base.Entity
{
    public interface IModificationAuditedEntity
    {
        int? LastModifierUserId { get; set; }
        DateTime? LastModificationTime { get; set; }
        User LastModifierUser { get; set; }
    }
}
