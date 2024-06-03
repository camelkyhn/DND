namespace DND.Middleware.Base.Dto
{
    public interface IIdentifiedDto<TKey>
    {
        TKey Id { get; set; }
    }
}
