namespace DND.Middleware.Base
{
    public interface IIdentifiedDto<TKey>
    {
        TKey Id { get; set ; }
    }

    public class IdentifiedDto<TKey> : IIdentifiedDto<TKey>
    {
        public TKey Id { get; set ; }
    }
}
