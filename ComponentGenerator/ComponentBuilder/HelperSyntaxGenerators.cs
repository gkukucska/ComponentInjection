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
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true,Inherited =true)]
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

    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true,Inherited =true)]
    internal class KeylessComponentAttribute: Attribute 
    {{
        private readonly Type _optionType;
        private readonly ServiceLifetime _lifetime;
        private readonly Type[] _implementationTypeCollection;

        public KeylessComponentAttribute(Type optionType, ServiceLifetime lifetime, params Type[] implementationTypeCollection)
        {{
            _implementationTypeCollection = implementationTypeCollection;
            _optionType = optionType;
            _lifetime = lifetime;
        }}
    }}

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

    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true,Inherited =true)]
    internal class KeyedServiceAttribute: Attribute 
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
    
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple =false,Inherited =true)]
    internal class AliasAttribute: Attribute
    {{
    }}

}}");
        }
    }
}