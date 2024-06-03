namespace DND.Middleware.Base.Dto
{
    public class IdentifiedDto<TKey> : IIdentifiedDto<TKey>
    {
        public TKey Id { get; set; }
    }
}
