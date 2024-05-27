﻿using DND.Middleware.Base;
using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Filter.Identity
{
    public class PermissionFilterDto : FilterDto
    {
        [StringLength(MaxLengths.LongText)]
        public string Name { get; set; }
    }
}
