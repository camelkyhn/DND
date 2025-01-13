using System;
using System.Collections.Generic;

namespace DND.Middleware.System
{
    public class TrackableEntity
    {
        public Type EntityType { get; set; }
        public List<string> IgnoredProperties { get; set; }
    }
}