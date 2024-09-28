using ComponentGenerator.Common.Models.Injectables;
using ComponentGenerator.Common.Models.Parameters;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ComponentGenerator.ComponentBuilder
{

    internal static class ComponentBuilderGeneratorHelpers
    {

        internal static void GenerateComponentBuilderSyntax(SourceProductionContext context, ComponentModel model)
        {
            if (model is null)
            {
                return;
            }

            var builderExtensionSyntax = $@"//compiler generated
#nullable disable
using System.CodeDom.Compiler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilderExtensions
{{
    public static partial class BuilderExtensions
    {{
        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        public static IHostApplicationBuilder InstallAsComponent_{Helpers.ToSnakeCase(model.ClassName)}(this IHostApplicationBuilder builder, string key)
        {{
            builder.Services.AddOptions<{model.OptionType}>(key).Bind(builder.Configuration.GetSection(key));
            builder.Services.AddKeyed{Helpers.GetLifeTimeSyntax(model.Lifetime)}<{model.ClassName}, {model.ClassName}>(key, {Helpers.ToSnakeCase(model.ClassName)}Factory);
            {GenerateProxyFactoryRegistrationSyntax(model)}
            return builder;
        }}

        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        private static {model.ClassName} {Helpers.ToSnakeCase(model.ClassName)}Factory(IServiceProvider provider, object? key)
        {{
            var snapshot = provider.GetRequiredService<IOptionsSnapshot<{model.OptionType}>>();
            var options = snapshot.Get(key?.ToString());

{GenerateConstructorParameterInitializationSyntax(model)}

            return new {model.ClassName}({GenerateConstructorSyntax(model)});
        }}


        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        private static {model.ClassName} {Helpers.ToSnakeCase(model.ClassName)}ProxyFactory(IServiceProvider provider, object? key)
        {{
            return provider.GetRequiredKeyedService<{model.ClassName}>(key);
        }}
    }}
}}
            ";
            context.AddSource($"{Helpers.ToSnakeCase(model.ClassName)}_BuilderExtensions.g.cs", builderExtensionSyntax);
        }

        private static string GenerateProxyFactoryRegistrationSyntax(ComponentModel model)
        {
            var builder = new StringBuilder();
            foreach (var implementation in model.ImplementationCollection)
            {
                builder.AppendLine($@"builder.Services.AddKeyed{Helpers.GetLifeTimeSyntax(model.Lifetime)}<{implementation}, {model.ClassName}>(key, {Helpers.ToSnakeCase(model.ClassName)}ProxyFactory);");
            }
            return builder.ToString();
        }

        internal static string GenerateConstructorParameterInitializationSyntax(ComponentModel model)
        {
            var builder = new StringBuilder();
            foreach (var parameter in model.Constructor.Parameters)
            {
                if (parameter is AliasParameterModel aliasParameterModel)
                {
                    builder.AppendLine($@"            var {aliasParameterModel.Name} = provider.Get{GenerateRequiredSyntaxIfNeeded(parameter)}KeyedService<{aliasParameterModel.Type}>(options.{Helpers.CapitalizeFirstLetter(aliasParameterModel.Name)});");
                    continue;
                }
                if (parameter is KeyedServiceParameterModel keyedParameterModel)
                {
                    builder.AppendLine($@"            var {keyedParameterModel.Name} = provider.Get{GenerateRequiredSyntaxIfNeeded(parameter)}KeyedService<{keyedParameterModel.Type}>({keyedParameterModel.ServiceKey});");
                    continue;
                }
                if (parameter.Type == model.OptionType)
                {
                    continue;
                }
                if (parameter is ServiceParameterModel serviceParameterModel)
                {
                    builder.AppendLine($@"            var {serviceParameterModel.Name} = provider.Get{GenerateRequiredSyntaxIfNeeded(parameter)}Service<{serviceParameterModel.Type}>();");
                    continue;
                }
            }
            return builder.ToString();
        }

        internal static string GenerateRequiredSyntaxIfNeeded(ParameterModelBase parameterModel)
        {
            return parameterModel.IsOptional ? string.Empty : "Required";
        }

        internal static string GenerateConstructorSyntax(ComponentModel model)
        {
            var parameterSyntaxCollection = new List<string>();
            foreach (var parameter in model.Constructor.Parameters)
            {
                if (parameter is AliasParameterModel)
                {
                    parameterSyntaxCollection.Add(parameter.Name);
                    continue;
                }
                if (parameter is ServiceKeyParameterModel)
                {
                    parameterSyntaxCollection.Add("key");
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

        internal static void GenerateComponentOptionSyntax(SourceProductionContext context, ComponentModel model)
        {
            if (!model.Constructor.Parameters.OfType<AliasParameterModel>().Any())
            {
                return;
            }


            var optionClassName = model.OptionType.Split('.').Last();
            var optionNamespace = string.Join(".", model.OptionType.Split('.').Take(model.OptionType.Split('.').Count() - 1));
            var optionSyntax = $@"//compiler generated
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace {optionNamespace}
{{

    [CompilerGenerated]
    [ExcludeFromCodeCoverage]
    [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
    partial class {optionClassName}
    {{
        {GenerateAliasProperties(model)}
    }}
}}
";

            context.AddSource($"{optionClassName}.g.cs", optionSyntax);
        }

        private static string GenerateAliasProperties(ComponentModel model)
        {
            var aliasProperties = model.Constructor.Parameters.OfType<AliasParameterModel>().Select(x =>
$@"        public string {Helpers.CapitalizeFirstLetter(x.Name)} {{ get; set; }}");

            return string.Join("\n", aliasProperties);
        }
    }
}