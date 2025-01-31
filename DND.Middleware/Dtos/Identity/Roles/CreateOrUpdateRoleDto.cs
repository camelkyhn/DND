﻿using System.ComponentModel.DataAnnotations;
using DND.Middleware.Base.Dto;
using DND.Middleware.Constants;

namespace DND.Middleware.Dtos.Identity.Roles
{
    public class CreateOrUpdateRoleDto : EntityDto<short?>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        public string Name { get; set; }
    }
}