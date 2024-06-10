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

            {GenerateInstallationSyntax(model)}

            return builder;

        }}

    }}
}}
";
        context.AddSource("ComponentBuilderExtensions.g.cs", builderExtensionSyntax);
    }

    internal static string GenerateInstallationSyntax(ApplicationModel model)
    {
        return string.Join(string.Empty, model.ReferencedComponents.Select(x => x.Split('.').Last()).Select(x => $@"
            foreach (var alias in aliases.Where(x => x.Value == ""{x}"").Select(x=>x.Key.Replace(""{model.ComponentSection}:"",string.Empty)))
            {{
                builder.Install{x}(alias);
            }}"));
    }
}