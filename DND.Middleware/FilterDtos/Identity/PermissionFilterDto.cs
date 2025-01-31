﻿using DND.Middleware.Base.Filter;
using DND.Middleware.Constants;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.FilterDtos.Identity
{
    public class PermissionFilterDto : FilterDto
    {
        [StringLength(MaxLengths.LongText)]
        public string Name { get; set; }
    }
}
