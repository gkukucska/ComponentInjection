using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SimpleComponent;
using System.Text;

namespace ComponentGenerator
{
    public static class SimpleComponent_ValidationHelper
    {
        public static void Validate_SimpleComponent_MyComponent(IHostApplicationBuilder builder, string aliasKey, StringBuilder errorCollection)
        {
            var configurationSection = builder.Configuration.GetSection(aliasKey);
            var otherComponentAlias = configurationSection?.GetValue<string>("OtherComponent");
            if (string.IsNullOrEmpty(otherComponentAlias))
            {
                errorCollection.AppendLine($"Missing value of OtherComponent from configuration of MyComponent: {aliasKey}");
            }
            else
            {
                builder.FindService<IMyOtherComponent>(otherComponentAlias, errorCollection);
            }
        }
        public static void Validate_SimpleComponent_MyOtherComponent(IHostApplicationBuilder builder, string aliasKey, StringBuilder errorCollection)
        {
            
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