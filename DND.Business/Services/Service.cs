using AutoMapper;
using DND.Middleware.Identity;
using DND.Middleware.System;
using DND.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DND.Business.Services
{
    public abstract class Service
    {
        protected readonly IMapper Mapper;
        protected readonly IAppSession AppSession;
        protected readonly IRepositoryContext RepositoryContext;

        public Service(IServiceProvider serviceProvider)
        {
            Mapper = serviceProvider.GetService<IMapper>();
            AppSession = serviceProvider.GetService<IAppSession>();
            RepositoryContext = serviceProvider.GetService<IRepositoryContext>();
        }

        public InputModelStateDictionary GetInputModelStateDictionary(object input)
        {
            var inputModelStateDictionary = new InputModelStateDictionary();
            var properties = input.GetType().GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes().Where(a => a is ValidationAttribute);
                foreach (var attribute in attributes)
                {
                    var result = ((ValidationAttribute)attribute).GetValidationResult(property.GetValue(input), new ValidationContext(input) { MemberName = property.Name });
                    if (!string.IsNullOrEmpty(result?.ErrorMessage))
                    {
                        if (inputModelStateDictionary.ContainsKey(property.Name))
                        {
                            inputModelStateDictionary[property.Name].Add(result.ErrorMessage);
                        }
                        else
                        {
                            inputModelStateDictionary.Add(property.Name, new List<string> { result.ErrorMessage });
                        }
                    }
                }
            }

            return inputModelStateDictionary;
        }
    }
}
