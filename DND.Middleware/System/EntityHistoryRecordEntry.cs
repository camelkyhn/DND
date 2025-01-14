﻿using DND.Middleware.Entities.System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
            var trackableEntity = EntityConfiguration.GetTrackableEntityList().FirstOrDefault(te => te.EntityType.Name == EntityName);
            if (trackableEntity != null)
            {
                foreach (var property in trackableEntity.IgnoredProperties)
                {
                    Values.Remove(property);
                }
            }

            return new EntityHistoryRecord
            {
                EntityName = EntityName,
                DateTime = DateTime.UtcNow,
                EntityId = EntityId,
                JsonValue = JsonSerializer.Serialize(Values)
            };
        }
    }
}
