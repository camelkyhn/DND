namespace DND.Middleware.Base.Dto
{
    public class BaseViewDto
    {
        public EntityAuditorForViewDto Creator { get; set; }
        public EntityAuditorForViewDto LastModifier { get; set; }
        public bool IsCreatedByCurrentUser { get; set; }
    }
}