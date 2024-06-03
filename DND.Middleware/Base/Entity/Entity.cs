namespace DND.Middleware.Base.Entity
{
    public class Entity<TKey> : Identified<TKey>, IEntity<TKey>
    {
        public bool IsDeleted { get; set; }
    }
}
