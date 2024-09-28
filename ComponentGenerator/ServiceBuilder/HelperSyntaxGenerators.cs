using Microsoft.CodeAnalysis;

namespace ComponentGenerator.ServiceBuilder
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
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true,Inherited =true)]
    internal class ServiceAttribute: Attribute 
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
