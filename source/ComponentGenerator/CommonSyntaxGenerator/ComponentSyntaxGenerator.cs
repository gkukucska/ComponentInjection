using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ComponentGenerator.CommonSyntaxGenerator
{
    [Generator]
    internal class ComponentSyntaxGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(HelperSyntaxGenerators.GenerateAttributes);
        }
    }

    internal static class HelperSyntaxGenerators
    {
        public static void GenerateAttributes(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("ComponentBaseAttributes.g.cs",
                $@"using System;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentGenerator 
{{    
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple = false,Inherited = true)]
    internal class AliasAttribute: Attribute
    {{
    }}
    
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple = false,Inherited = true)]
    internal class AliasCollectionAttribute: Attribute
    {{
    }}
    
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple = false,Inherited = true)]
    internal class OptionalAttribute: Attribute
    {{
    }}
    
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = true)]
    internal abstract class ComponentAttributeBase: Attribute
    {{
    }}

}}");
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
