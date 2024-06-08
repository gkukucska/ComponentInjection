using ComponentGenerator.ComponentBuilder.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace ComponentGenerator.ComponentBuilder
{

    internal static class ComponentBuilderGeneratorHelpers
    {

        internal static string GenerateAliasResolvingSyntax(ComponentModel model)
        {
            var resolvingSyntaxCollection = model.Constructor.Parameters.Where(x => x.IsAlias && x.Type != model.OptionType).Select(x => $@"
            options.{Helpers.CapitalizeFirstLetter(x.Name)}.Value = provider.GetRequiredKeyedService<{x.Type}>(options.{Helpers.CapitalizeFirstLetter(x.Name)}.Key);");
            return string.Join("\n", resolvingSyntaxCollection);
        }

        internal static void GenerateComponentBuilderSyntax(SourceProductionContext context, ComponentModel model)
        {
            if (model is null)
            {
                return;
            }

            var builderExtensionSyntax = $@"//compiler generated
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using {model.Namespace};

namespace ComponentBuilderExtensions
{{
    public static partial class BuilderExtensions
    {{
        public static IHostApplicationBuilder Install{model.ClassName}(this IHostApplicationBuilder hostBuilder, string key)
        {{
            hostBuilder.Services.AddOptions<{model.OptionType}>().Bind(hostBuilder.Configuration.GetSection(key))
                                                                 .ValidateOnStart()
                                                                 .PostConfigure<IServiceProvider>(PostConfigure{model.ClassName}Options);
            hostBuilder.Services.AddKeyedSingleton<{model.InterfaceType}, {model.ClassName}>(key, {model.ClassName}Factory);
            return hostBuilder;
        }}

        private static void PostConfigure{model.ClassName}Options({model.OptionType} options, IServiceProvider provider)
        {{
            {GenerateAliasResolvingSyntax(model)}
        }}

        private static {model.ClassName} {model.ClassName}Factory(IServiceProvider provider, object? key)
        {{
            var snapshot = provider.GetRequiredService<IOptionsSnapshot<{model.OptionType}>>();
            var options = snapshot.Get(key.ToString());

            {GenerateConstructorParameterInitializationSyntax(model)}

            return new {model.ClassName}({GenerateConstructorSyntax(model)});
        }}
    }}
}}
";
            context.AddSource($"{model.ClassName}BuilderExtensions.g.cs", builderExtensionSyntax);
        }

        internal static void GenerateComponentOptionSyntax(SourceProductionContext context, ComponentModel model)
        {
            if (model?.Constructor.Parameters.All(x => !x.IsAlias) ?? true)
            {
                return;
            }


            var optionClassName = model.OptionType.Split('.').Last();
            var optionNamespace = string.Join(".", model.OptionType.Split('.').Take(model.OptionType.Split('.').Count() - 1));

            var optionSyntax = $@"//compiler generated
using System.Runtime.Serialization;

namespace {optionNamespace}
{{
    public partial class {optionClassName}
    {{
        {GenerateAliasProperties(model)}
    }}

    public class Alias<T>
    {{
        [DataMember]
        public string Key {{ get; set; }}

        [IgnoreDataMember]
        public T Value {{ get; set; }}
    }}
}}
";

            context.AddSource($"{optionClassName}.g.cs", optionSyntax);
        }

        private static string GenerateAliasProperties(ComponentModel model)
        {
            var aliasProperties = model.Constructor.Parameters.Where(x => x.IsAlias).Select(x =>
$@"        public Alias<{x.Type}> {Helpers.CapitalizeFirstLetter(x.Name)} {{ get; set; }}");

            return string.Join("\n", aliasProperties);
        }

        internal static string GenerateConstructorParameterInitializationSyntax(ComponentModel model)
        {
            var resolvingSyntaxCollection = model.Constructor.Parameters.Where(x => !x.IsAlias && x.Type != model.OptionType).Select(x => $@"
            var {x.Name} = provider.GetRequiredService<{x.Type}>();");
            return string.Join("\n", resolvingSyntaxCollection);
        }

        internal static string GenerateConstructorSyntax(ComponentModel model)
        {
            var parameterSyntaxCollection = new List<string>();
            foreach (var parameter in model.Constructor.Parameters)
            {
                if (parameter.IsAlias)
                {
                    parameterSyntaxCollection.Add($"options.{Helpers.CapitalizeFirstLetter(parameter.Name)}.Value");
                    continue;
                }
                if (parameter.Type == model.OptionType)
                {
                    parameterSyntaxCollection.Add($"options");
                    continue;
                }
                parameterSyntaxCollection.Add(parameter.Name);
            }
            return string.Join(", ", parameterSyntaxCollection);
        }
    }
}