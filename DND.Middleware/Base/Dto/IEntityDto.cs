namespace DND.Middleware.Base.Dto
{
    public interface IEntityDto<TKey>
    {
        TKey Id { get; set; }
    }
}
