using DND.Middleware.Base;
using DND.Middleware.Constants;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Entity.System
{
    [Table(nameof(EntityHistoryRecord))]
    public class EntityHistoryRecord : Identified<long>
    {
        [Required]
        [StringLength(MaxLengths.ShortText, MinimumLength = MinLengths.ShortText)]
        public string EntityId { get; set; }

        [Required]
        [StringLength(MaxLengths.ShortText, MinimumLength = MinLengths.ShortText)]
        public string EntityName { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public string JsonValue { get; set; }
    }
}
