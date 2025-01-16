using ComponentGenerator.Common.Models;
using ComponentGenerator.Common.Models.Injectables;
using ComponentGenerator.Common.Models.Parameters;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace ComponentGenerator.ComponentBuilder
{
    internal static class ComponentValidatorGeneratorHelpers
    {
        internal static void GenerateComponentBuilderSyntax(SourceProductionContext context, ComponentModel model)
        {
            var validationSyntax = $@"//compiler generated
#nullable disable
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilderExtensions
{{
    public static partial class BuilderExtensions
    {{
        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        public static void Validate_{Helpers.ToSnakeCase(model.ClassName)}(IHostApplicationBuilder builder, string aliasKey, StringBuilder errorCollection)
        {{
            var configurationSection = builder.Configuration.GetSection(aliasKey);
{Helpers.GenerateConstructorValidationSyntax(model.Constructor,model.ClassName)}
        }}
    
    }}

}}";
            context.AddSource($"{Helpers.ToSnakeCase(model.ClassName)}_ValidatorExtensions.g.cs", validationSyntax);
        }
        internal static void GenerateHelperSyntax(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("ComponentValidatorGeneratorHelper.g.cs", $@"//compiler generated
#nullable disable
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilderExtensions
{{
    internal static partial class ValidationHelperExtensions
    {{
        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        internal static void FindService<TService>(this IHostApplicationBuilder builder, string key, StringBuilder errorCollection)
        {{
            var serviceCandidates = builder.Services.Where(x=>x.ServiceType == typeof(TService) && x.IsKeyedService).ToList();
            if (serviceCandidates.All(x => x.ServiceKey?.ToString() != key))
            {{
                errorCollection.AppendLine($""The service '{{typeof(TService)}}' with key {{key}} is not registered."");
                errorCollection.AppendLine($""Available candidates for service '{{typeof(TService)}}':"");
                foreach (var serviceDescriptor in serviceCandidates)
                {{
                    errorCollection.AppendLine($""\t{{serviceDescriptor.ServiceKey?.ToString()}}"");
                }}
            }}
        }}

        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        internal static void FindService<TService>(this IHostApplicationBuilder builder, StringBuilder errorCollection)
        {{
            if (!builder.Services.Any(x => x.ServiceType == typeof(TService) && !x.IsKeyedService))
            {{
                errorCollection.AppendLine($""No service for type '{{typeof(TService)}}' is registered."");
            }}
        }}
    }}
}}"
            );
        }
    }
}