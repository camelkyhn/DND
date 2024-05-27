namespace DND.Middleware.Base
{
    public interface IEntity<TKey> : IIdentified<TKey>
    {
        bool IsDeleted { get; set; }
    }

    public class Entity<TKey> : Identified<TKey>, IEntity<TKey>
    {
        public bool IsDeleted { get; set; }
    }
}
