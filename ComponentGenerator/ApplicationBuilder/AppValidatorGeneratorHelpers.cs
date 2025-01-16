using ComponentGenerator.ApplicationBuilder.Models;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace ComponentGenerator.ApplicationBuilder
{
    internal static class AppValidatorGeneratorHelpers
    {
        public static void GenerateApplicationValidatorSyntax(SourceProductionContext context, ApplicationModel model)
        {
            if (model is null)
            {
                return;
            }

            var builderExtensionSyntax = $@"//compiler generated
#nullable disable
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ComponentBuilderExtensions;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace {model.ApplicationNamespace}
{{
    internal static partial class BuilderExtensions
    {{
        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        internal static IHostApplicationBuilder ValidateAliases(this IHostApplicationBuilder builder)
        {{
            var aliases = builder.Configuration.GetRequiredSection(""{model.ComponentSection}"")
                                               .AsEnumerable();
            var errors = new StringBuilder();

{GenerateComponentValidationSyntax(model)}
{GenerateHostedServiceValidationSyntax(model)}
{GenerateKeylessComponentValidationSyntax(model)}
            
            if(!string.IsNullOrWhiteSpace(errors.ToString()))
                throw new Exception(errors.ToString());

            return builder;

        }}

    }}
}}
";
            context.AddSource("ApplicationValidatorExtensions.g.cs", builderExtensionSyntax);
        }

        private static string GenerateHostedServiceValidationSyntax(ApplicationModel model)
        {
            var syntax = new StringBuilder();
            foreach (var hostedService in model.HostedServices)
            {
                syntax.Append($@"
            builder.Validate_{Helpers.ToSnakeCase(hostedService)}(errors);");
            }
            return syntax.ToString();
        }

        private static string GenerateKeylessComponentValidationSyntax(ApplicationModel model)
        {
            var syntax = new StringBuilder();
            foreach (var keylessComponent in model.ReferencedKeylessComponents)
            {
                syntax.Append($@"
            builder.Validate_{Helpers.ToSnakeCase(keylessComponent)}(errors);");
            }
            return syntax.ToString();
        }

        private static string GenerateComponentValidationSyntax(ApplicationModel model)
        {
            var syntax = new StringBuilder();
            foreach (var component in model.ReferencedComponents)
            {
                syntax.Append($@"
            var {Helpers.ToSnakeCase(component)}_Aliases = aliases.Where(alias => alias.Value == ""{component}"");
            foreach(var alias in {Helpers.ToSnakeCase(component)}_Aliases)
            {{
                builder.Validate_{Helpers.ToSnakeCase(component)}(alias.Key,errors);
            }}");
            }
            return syntax.ToString();
        }
    }
}