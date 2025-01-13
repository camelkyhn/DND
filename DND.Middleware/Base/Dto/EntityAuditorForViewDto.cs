using System;

namespace DND.Middleware.Base.Dto
{
    public class EntityAuditorForViewDto
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? AuditedTime { get; set; }
    }
}