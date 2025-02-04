using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ComponentGenerator.ApplicationBuilder
{
    internal static class HelpersSyntaxGenerator
    {

        internal static void GenerateAttributes(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("ApplicationAttributes.g.cs",
$@"//compiler generated
#nullable disable
using System;
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ComponentGenerator 
{{
    [CompilerGenerated]
    [ExcludeFromCodeCoverage]
    [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = false)]
    internal class ApplicationAttribute: Attribute 
    {{
        private readonly string _componentSection;

        public ApplicationAttribute(string componentSection = ""Components"")
        {{
            _componentSection = componentSection;   
        }}
    }}

}}");
        }
    }
}