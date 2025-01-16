using ComponentGenerator;
using ComponentGenerator.ApplicationBuilder.Models;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Reflection;

internal static class AppBuilderGeneratorHelpers
{

    internal static void GenerateApplicationBuilderSyntax(SourceProductionContext context, ApplicationModel model)
    {
        if (model is null)
        {
            return;
        }

        var builderExtensionSyntax = $@"//compiler generated
#nullable disable
using System.CodeDom.Compiler;
using ComponentBuilderExtensions;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace {model.ApplicationNamespace}
{{
    internal static partial class BuilderExtensions
    {{
        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        internal static IHostApplicationBuilder InstallAliases(this IHostApplicationBuilder builder)
        {{
            var aliases = builder.Configuration.GetRequiredSection(""{model.ComponentSection}"")
                                               .AsEnumerable();

{GenerateServiceInstallationSyntax(model)}
{GenerateKeyedServiceInstallationSyntax(model)}
{GenerateComponentInstallationSyntax(model)}
{GenerateKeylessComponentInstallationSyntax(model)}
{GenerateHostedServiceInstallationSyntax(model)}

            return builder;

        }}

    }}
}}
";
        context.AddSource("ApplicationBuilderExtensions.g.cs", builderExtensionSyntax);
    }

    private static string GenerateServiceInstallationSyntax(ApplicationModel model)
    {
        return string.Join(string.Empty, model.ReferencedServices.Select(x => $@"
            builder.InstallAsService_{Helpers.ToSnakeCase(x)}();"));
    }

    private static string GenerateKeyedServiceInstallationSyntax(ApplicationModel model)
    {
        return string.Join(string.Empty, model.ReferencedKeyedServices.Select(x => $@"
            builder.InstallAsKeyedService_{Helpers.ToSnakeCase(x)}();"));
    }

    internal static string GenerateComponentInstallationSyntax(ApplicationModel model)
    {
        return string.Join(string.Empty, model.ReferencedComponents.Select(x => $@"
            foreach (var alias in aliases.Where(x => x.Value == ""{x}"").Select(x=>x.Key.Replace(""{model.ComponentSection}:"",string.Empty)))
            {{
                builder.InstallAsComponent_{Helpers.ToSnakeCase(x)}(alias);
            }}"));
    }

    internal static string GenerateKeylessComponentInstallationSyntax(ApplicationModel model)
    {
        return string.Join(string.Empty, model.ReferencedKeylessComponents.Select(x => $@"
            builder.InstallAsKeylessComponent_{Helpers.ToSnakeCase(x)}();"));
    }

    internal static string GenerateHostedServiceInstallationSyntax(ApplicationModel model)
    {
        return string.Join(string.Empty, model.HostedServices.Select(x => $@"
            builder.InstallAsHostedService_{Helpers.ToSnakeCase(x)}();"));
    }
}