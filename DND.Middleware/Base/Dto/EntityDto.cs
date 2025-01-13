namespace DND.Middleware.Base.Dto
{
    public class EntityDto<TKey> : IEntityDto<TKey>
    {
        public TKey Id { get; set; }
    }
}
