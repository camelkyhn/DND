using DND.Middleware.Entities.Identity;
using System;

namespace DND.Middleware.Base.Entity
{
    public interface ICreationAuditedEntity
    {
        int? CreatorUserId { get; set; }
        DateTime CreationTime { get; set; }
        User CreatorUser { get; set; }
    }
}
