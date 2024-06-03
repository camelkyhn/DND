using System;

namespace DND.Middleware.Base.Dto
{
    public interface IModificationAuditedEntityDto
    {
        int? ModifierUserId { get; set; }
        DateTimeOffset? ModificationTime { get; set; }
    }
}
