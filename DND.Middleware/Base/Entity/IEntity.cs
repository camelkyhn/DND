namespace DND.Middleware.Base.Entity
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
