using Microsoft.CodeAnalysis;

namespace ComponentGenerator.ServiceBuilder
{
    internal static class HelperSyntaxGenerators
    {
        internal static void GenerateAttributes(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("KeyedServiceAttributes.g.cs",
$@"using System;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentGenerator 
{{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = true)]
    internal class ServiceAttribute: ComponentAttributeBase 
    {{
        private readonly ServiceLifetime _lifetime;
        private readonly Type[] _implementationTypeCollection;

        public ServiceAttribute(ServiceLifetime lifetime, params Type[] implementationTypeCollection)
        {{
            _implementationTypeCollection = implementationTypeCollection;
            _lifetime = lifetime;
        }}
    }}
}}");
        }
    }
}
