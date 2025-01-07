using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using AutoMapper;
using DND.Middleware.Identity;
using DND.Middleware.System;
using DND.Storage;

namespace DND.Business.Services
{
    public class Service
    {
        protected readonly IMapper Mapper;
        protected readonly IAppSession AppSession;
        protected readonly IRepositoryContext RepositoryContext;

        public Service(IMapper mapper, IAppSession appSession, IRepositoryContext repositoryContext)
        {
            Mapper = mapper;
            AppSession = appSession;
            RepositoryContext = repositoryContext;
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
