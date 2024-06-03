namespace DND.Middleware.Base.Dto
{
    public interface IEntityDto<TKey> : IIdentifiedDto<TKey>
    {
        bool IsDeleted { get; set; }
    }
}
