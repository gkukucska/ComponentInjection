using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SimpleComponent;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ComponentGenerator
{
    public static class ValidationHelper
    {
        public static IHostApplicationBuilder Validate(this IHostApplicationBuilder builder)
        {
            var errors = new StringBuilder();
            
            var components = builder.Configuration.GetRequiredSection("Components").Get<Dictionary<string, string>>();
            
            if (components is null)
                throw new Exception("Component section is missing from configuration");
            
            var simpleComponent_MyComponent_AliasCollection = components.Where(x=>x.Value=="SimpleComponent.MyComponent");

            foreach (var alias in simpleComponent_MyComponent_AliasCollection)
            {
                SimpleComponent_ValidationHelper.Validate_SimpleComponent_MyComponent(builder,alias.Key, errors);
            }
            
            var simpleComponent_MyOtherComponent_AliasCollection = components.Where(x=>x.Value=="SimpleComponent.MyOtherComponent");

            foreach (var alias in simpleComponent_MyOtherComponent_AliasCollection)
            {
                SimpleComponent_ValidationHelper.Validate_SimpleComponent_MyOtherComponent(builder,alias.Key, errors);
            }

            Validate_SimpleApplication_MyHostedService(builder,errors);

            if (!string.IsNullOrEmpty(errors.ToString()))
            {
                throw new ValidationException(errors.ToString());
            }
            return builder;
        }
        
        private static void Validate_SimpleApplication_MyHostedService(IHostApplicationBuilder builder, StringBuilder errorCollection)
        {
            var configurationSection = builder.Configuration.GetRequiredSection("SimpleApplication.MyHostedService");
            var myComponentAlias = configurationSection.GetValue<string>("MyComponentAlias");
            if (string.IsNullOrEmpty(myComponentAlias))
            {
                errorCollection.AppendLine($"Missing value of MyComponent from configuration of MyHostedService");
            }
            else
            {
                builder.FindService<IMyComponent>(myComponentAlias, errorCollection);
            }
        }

        private static void FindService<TService>(this IHostApplicationBuilder builder, string key, StringBuilder errorCollection)
        {
            var serviceCandidates = builder.Services.Where(x=>x.ServiceType == typeof(TService) && x.IsKeyedService).ToList();
            if (serviceCandidates.All(x => x.ServiceKey?.ToString() != key))
            {
                errorCollection.AppendLine($"The service '{typeof(TService)}' with key {key} is not registered.");
                errorCollection.AppendLine($"Available candidates for service '{typeof(TService)}':");
                foreach (var serviceDescriptor in serviceCandidates)
                {
                    errorCollection.AppendLine($"\t{serviceDescriptor.ServiceKey?.ToString()}");
                }
            }
        }

        private static void FindService<TService>(this IHostApplicationBuilder builder, StringBuilder errorCollection)
        {
            if (!builder.Services.Any(x => x.ServiceType == typeof(TService) && !x.IsKeyedService))
            {
                errorCollection.AppendLine($"No service for type '{typeof(TService)}' is registered.");
            }
        }
    }
}