﻿using DND.Middleware.Entity.System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DND.Middleware.System
{
    public class EntityHistoryRecordEntry
    {
        public Dictionary<string, object> Values { get; } = new();
        public List<PropertyEntry> TemporaryProperties { get; } = new();
        public string EntityName { get; init; }
        public string EntityId { get; set; }

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public EntityHistoryRecord ToEntityHistoryRecord()
        {
            foreach (var property in EntityConfiguration.IgnoredProperties)
            {
                Values.Remove(property);
            }

            return new EntityHistoryRecord
            {
                EntityName = EntityName,
                DateTime = DateTime.UtcNow,
                EntityId = EntityId,
                JsonValue = JsonConvert.SerializeObject(Values)
            };
        }
    }
}
