using DND.Middleware.Entity.Identity;
using System;

namespace DND.Middleware.Base.Entity
{
    public interface ICreationAuditedEntity
    {
        int CreatorUserId { get; set; }
        DateTimeOffset CreationTime { get; set; }
        User CreatorUser { get; set; }
    }
}
