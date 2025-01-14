﻿using DND.Middleware.Constants;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DND.Middleware.Base.Entity;

namespace DND.Middleware.Entities.System
{
    [Table(nameof(EntityHistoryRecord))]
    public class EntityHistoryRecord : Entity<long>
    {
        [Required]
        [StringLength(MaxLengths.ShortText, MinimumLength = MinLengths.ShortText)]
        public string EntityId { get; set; }

        [Required]
        [StringLength(MaxLengths.ShortText, MinimumLength = MinLengths.ShortText)]
        public string EntityName { get; set; }

        public DateTime DateTime { get; set; }

        public string JsonValue { get; set; }
    }
}
