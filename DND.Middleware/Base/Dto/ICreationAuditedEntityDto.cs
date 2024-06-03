using System;

namespace DND.Middleware.Base.Dto
{
    public interface ICreationAuditedEntityDto
    {
        int CreatorUserId { get; set; }
        DateTimeOffset CreationTime { get; set; }
    }
}
