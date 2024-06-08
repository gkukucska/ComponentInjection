using Microsoft.CodeAnalysis;

namespace ComponentGenerator.ComponentBuilder
{
    internal static class HelperSyntaxGenerators
    {

        internal static void GenerateAttributes(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("ComponentAttributes.g.cs",
$@"using System;

namespace ComponentGenerator 
{{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true,Inherited =true)]
    internal class ComponentAttribute: Attribute 
    {{
        private readonly Type _interfaceType;
        private readonly Type _optionType;
        private readonly Lifetime _lifetime;

        public ComponentAttribute(Type interfaceType, Type optionType, Lifetime lifetime)
        {{
            _interfaceType = interfaceType;
            _optionType = optionType;
            _lifetime = lifetime;
        }}
    }}
    
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple =false,Inherited =true)]
    internal class AliasAttribute: Attribute
    {{
    }}

    internal enum Lifetime
    {{
        Singleton,
        Transient,
        Scoped
    }}

}}");
        }
    }
}