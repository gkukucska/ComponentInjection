﻿using ComponentGenerator;
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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ComponentBuilderExtensions;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace {model.ApplicationNamespace}
{{
    internal static class BuilderExtensions
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

            return builder;

        }}

    }}
}}
";
        context.AddSource("ComponentBuilderExtensions.g.cs", builderExtensionSyntax);
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
}