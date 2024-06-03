namespace DND.Middleware.Base.Entity
{
    public interface IEntity<TKey> : IIdentified<TKey>
    {
        bool IsDeleted { get; set; }
    }
}
