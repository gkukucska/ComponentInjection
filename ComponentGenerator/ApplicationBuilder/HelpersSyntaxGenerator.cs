using Microsoft.CodeAnalysis;

namespace ComponentGenerator.ApplicationBuilder
{
    internal static class HelpersSyntaxGenerator
    {

        internal static void GenerateAttributes(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("ApplicationAttributes.g.cs",
$@"using System;

namespace ComponentGenerator 
{{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = false)]
    internal class ApplicationAttribute: Attribute 
    {{
        private readonly string _componentSection;

        public ApplicationAttribute(string componentSection)
        {{
            _componentSection = componentSection;   
        }}
    }}

}}");
        }
    }
}