using ComponentGenerator.Common.Models.Injectables;
using ComponentGenerator.Common.Models.Parameters;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ComponentGenerator.KeylessComponentBuilder
{

    internal static class KeylessComponentBuilderGeneratorHelpers
    {

        internal static void GenerateKeylessComponentBuilderSyntax(SourceProductionContext context, KeylessComponentModel model)
        {
            if (model is null)
            {
                return;
            }

            var builderExtensionSyntax = $@"//compiler generated
#nullable disable
using System.Linq;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
        public static IHostApplicationBuilder InstallAsKeylessComponent_{Helpers.ToSnakeCase(model.ClassName)}(this IHostApplicationBuilder builder)
        {{
            builder.Services.AddOptions<{model.OptionType}>(""{Helpers.ToSnakeCase(model.ClassName)}"").Bind(builder.Configuration.GetSection(""{Helpers.ToSnakeCase(model.ClassName)}""));
            builder.Services.AddKeyed{Helpers.GetLifeTimeSyntax(model.Lifetime)}<{model.ClassName}, {model.ClassName}>(""{Helpers.ToSnakeCase(model.ClassName)}"", {Helpers.ToSnakeCase(model.ClassName)}Factory);
            {GenerateProxyFactoryRegistrationSyntax(model)}
            return builder;
        }}

        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        private static {model.ClassName} {Helpers.ToSnakeCase(model.ClassName)}Factory(IServiceProvider provider, object key)
        {{
            var snapshot = provider.GetRequiredService<IOptionsSnapshot<{model.OptionType}>>();
            var options = snapshot.Get(key?.ToString());

{ComponentBuilder.ComponentBuilderGeneratorHelpers.GenerateConstructorParameterInitializationSyntax(model)}

            return new {model.ClassName}({GenerateConstructorSyntax(model)});
        }}


        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        private static {model.ClassName} {Helpers.ToSnakeCase(model.ClassName)}ProxyFactory(IServiceProvider provider)
        {{
            return provider.GetRequiredKeyedService<{model.ClassName}>(""{Helpers.ToSnakeCase(model.ClassName)}"");
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
                builder.AppendLine($@"builder.Services.Add{Helpers.GetLifeTimeSyntax(model.Lifetime)}<{implementation}, {model.ClassName}>({Helpers.ToSnakeCase(model.ClassName)}ProxyFactory);");
            }
            return builder.ToString();
        }

        internal static string GenerateConstructorSyntax(ComponentModel model)
        {
            var parameterSyntaxCollection = new List<string>();
            foreach (var parameter in model.Constructor.Parameters)
            {
                if (parameter is AliasParameterModel || parameter is AliasCollectionParameterModel)
                {
                    parameterSyntaxCollection.Add(parameter.Name);
                    continue;
                }
                if (parameter is ServiceKeyParameterModel)
                {
                    parameterSyntaxCollection.Add(Helpers.ToSnakeCase(model.ClassName));
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

        internal static void GenerateKeylessComponentOptionSyntax(SourceProductionContext context, ComponentModel model)
        {
            if (!model.Constructor.Parameters.OfType<AliasParameterModel>().Any() && !model.Constructor.Parameters.OfType<AliasCollectionParameterModel>().Any())
            {
                return;
            }


            var optionClassName = model.OptionType.Split('.').Last();
            var optionNamespace = string.Join(".", model.OptionType.Split('.').Take(model.OptionType.Split('.').Count() - 1));
            var optionSyntax = $@"//compiler generated
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace {optionNamespace}
{{

    [CompilerGenerated]
    [ExcludeFromCodeCoverage]
    [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
    partial class {optionClassName}
    {{
{ComponentBuilder.ComponentBuilderGeneratorHelpers.GenerateAliasProperties(model)}
    }}
}}
";

            context.AddSource($"{optionClassName}.g.cs", optionSyntax);
        }
    }
}