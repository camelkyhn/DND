namespace DND.Middleware.Base
{
    public interface IEntityDto<TKey> : IIdentifiedDto<TKey>
    {
        bool IsDeleted { get; set; }
    }

    public class EntityDto<TKey> : IdentifiedDto<TKey>, IEntityDto<TKey>
    {
        public bool IsDeleted { get; set; }
    }
}
