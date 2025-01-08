using Microsoft.CodeAnalysis;

namespace ComponentGenerator.HostedServiceBuilder
{
    internal static class HelperSyntaxGenerators
    {

        internal static void GenerateAttributes(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("HostedServiceAttribute.g.cs",
$@"using System;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentGenerator 
{{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = true)]
    internal class HostedServiceAttribute: ComponentAttributeBase 
    {{
        private readonly Type _optionType;

        public HostedServiceAttribute(Type optionType)
        {{
            _optionType = optionType;
        }}
    }}

}}");
        }
    }
}