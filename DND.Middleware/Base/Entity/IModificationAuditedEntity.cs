using DND.Middleware.Entity.Identity;
using System;

namespace DND.Middleware.Base.Entity
{
    public interface IModificationAuditedEntity
    {
        int? ModifierUserId { get; set; }
        DateTimeOffset? ModificationTime { get; set; }
        User ModifierUser { get; set; }
    }
}
