using Microsoft.CodeAnalysis;

namespace ComponentGenerator.ComponentBuilder
{
    internal static class HelperSyntaxGenerators
    {

        internal static void GenerateAttributes(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("ComponentAttributes.g.cs",
$@"using System;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentGenerator 
{{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = true)]
    internal class ComponentAttribute: Attribute 
    {{
        private readonly Type _optionType;
        private readonly ServiceLifetime _lifetime;
        private readonly Type[] _implementationTypeCollection;

        public ComponentAttribute(Type optionType, ServiceLifetime lifetime, params Type[] implementationTypeCollection)
        {{
            _implementationTypeCollection = implementationTypeCollection;
            _optionType = optionType;
            _lifetime = lifetime;
        }}
    }}
    
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple = false,Inherited = true)]
    internal class AliasAttribute: Attribute
    {{
    }}
    
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple = false,Inherited = true)]
    internal class OptionalAttribute: Attribute
    {{
    }}

}}");
        }
    }
}