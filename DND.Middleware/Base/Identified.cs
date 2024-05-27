using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Base
{
    public interface IIdentified<TKey>
    {
        TKey Id { get; set; }
    }

    public abstract class Identified<TKey> : IIdentified<TKey>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }
    }
}
