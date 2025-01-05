using System;

namespace DND.Middleware.Base.Dto
{
    public interface IModificationAuditedEntityDto
    {
        int? ModifierUserId { get; set; }
        DateTime? ModificationTime { get; set; }
    }
}
