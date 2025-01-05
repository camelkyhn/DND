using System;

namespace DND.Middleware.Base.Dto
{
    public interface ICreationAuditedEntityDto
    {
        int CreatorUserId { get; set; }
        DateTime CreationTime { get; set; }
    }
}
