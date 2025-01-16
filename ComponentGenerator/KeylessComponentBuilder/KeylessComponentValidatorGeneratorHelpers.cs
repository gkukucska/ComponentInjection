using ComponentGenerator.Common.Models;
using ComponentGenerator.Common.Models.Injectables;
using ComponentGenerator.Common.Models.Parameters;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace ComponentGenerator.KeylessComponentBuilder
{
    internal static class KeylessComponentValidatorGeneratorHelpers
    {
        internal static void GenerateComponentBuilderSyntax(SourceProductionContext context, KeylessComponentModel model)
        {
            var validationSyntax = $@"//compiler generated
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilderExtensions
{{
    public static partial class BuilderExtensions
    {{
        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        public static void Validate_{Helpers.ToSnakeCase(model.ClassName)}(IHostApplicationBuilder builder, string aliasKey, StringBuilder errorCollection)
        {{
            var configurationSection = builder.Configuration.GetSection(""{model.ClassName}"");
{Helpers.GenerateConstructorValidationSyntax(model.Constructor,model.ClassName)}
        }}
    
    }}

}}";
            context.AddSource($"{Helpers.ToSnakeCase(model.ClassName)}_ValidatorExtensions.g.cs", validationSyntax);
        }
    }
}