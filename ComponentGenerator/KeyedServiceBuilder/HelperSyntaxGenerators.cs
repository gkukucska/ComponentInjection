using Microsoft.CodeAnalysis;

namespace ComponentGenerator.KeyedServiceBuilder
{
    internal static class HelperSyntaxGenerators
    {
        internal static void GenerateAttributes(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("ServiceAttributes.g.cs",
$@"using System;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentGenerator 
{{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = true)]
    internal class KeyedServiceAttribute: ComponentAttributeBase 
    {{
        private readonly string _serviceKey;
        private readonly ServiceLifetime _lifetime;
        private readonly Type[] _implementationTypeCollection;

        public KeyedServiceAttribute(string serviceKey, ServiceLifetime lifetime, params Type[] implementationTypeCollection)
        {{
            _serviceKey = serviceKey;
            _implementationTypeCollection = implementationTypeCollection;
            _lifetime = lifetime;
        }}
    }}
}}");
        }
    }
}
