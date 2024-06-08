using ComponentGenerator.ApplicationBuilder.Model;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

internal static class AppBuilderGeneratorHelpers
{

    internal static void GenerateApplicationBuilderSyntax(SourceProductionContext context, ApplicationModel model)
    {
        if (model is null)
        {
            return;
        }
        var builderExtensionSyntax = $@"//compiler generated
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ComponentBuilderExtensions;

namespace SimpleApplication
{{
    internal static class BuilderExtensions
    {{
        internal static IHostApplicationBuilder InstallAliases(this IHostApplicationBuilder builder)
        {{
            var aliases = builder.Configuration.GetRequiredSection(""{model.ComponentSection}"")
                                                 .AsEnumerable();
            return builder{GenerateInstallationSyntax(model.ReferencedComponents)};

        }}

        private static IHostApplicationBuilder InstallAliases<T>(this IHostApplicationBuilder builder,IEnumerable<KeyValuePair<string, string>> aliases)
        {{
            foreach (var alias in aliases.Where(x => x.Value.Equals(typeof(T).Name)))
            {{
                builder.InstallMyComponent(alias.Key);
            }}
            return builder;
        }}

    }}
}}
";
        context.AddSource("ComponentBuilderExtensions.g.cs", builderExtensionSyntax);
    }

    internal static string GenerateInstallationSyntax(List<string> referencedComponents)
    {
        return string.Join("\n                          ", referencedComponents.Select(x => $@".InstallAliases<{x}>(aliases)"));
    }
}