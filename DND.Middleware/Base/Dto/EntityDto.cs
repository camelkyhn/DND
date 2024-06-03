using DND.Middleware.Base.Entity;

namespace DND.Middleware.Base.Dto
{
    public class EntityDto<TKey> : IdentifiedDto<TKey>, IEntityDto<TKey>
    {
        public bool IsDeleted { get; set; }
    }
}
