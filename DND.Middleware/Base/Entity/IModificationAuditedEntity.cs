using DND.Middleware.Entities.Identity;
using System;

namespace DND.Middleware.Base.Entity
{
    public interface IModificationAuditedEntity
    {
        int? ModifierUserId { get; set; }
        DateTime? ModificationTime { get; set; }
        User ModifierUser { get; set; }
    }
}
