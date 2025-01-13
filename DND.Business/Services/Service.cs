using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using DND.Middleware.Web;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DND.Business.Services
{
    public abstract class Service
    {
        protected readonly IMapper Mapper;
        protected readonly AppSession AppSession;
        protected readonly ModelStateDictionary ModelState;

        public Service(IServiceProvider serviceProvider)
        {
            Mapper = serviceProvider.GetService<IMapper>();
            AppSession = serviceProvider.GetService<AppSession>();
            var actionContextAccessor = serviceProvider.GetService<IActionContextAccessor>();
            ModelState = actionContextAccessor.ActionContext?.ModelState;
        }
    }
}
