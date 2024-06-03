namespace DND.Middleware.Base.Entity
{
    public interface IIdentified<TKey>
    {
        TKey Id { get; set; }
    }
}
