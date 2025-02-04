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
        private static KeylessComponentModel _lastModel;
        private static KeyValuePair<string, string> _lastAction;

        internal static void GenerateKeylessComponentBuilderSyntax(SourceProductionContext context, KeylessComponentModel model)
        {
            if (model is null)
            {
                return;
            }

            if (_lastModel != model)
            {


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
            builder.Services.AddOptions<{model.OptionType}>(""{model.ClassName}"").Bind(builder.Configuration.GetSection(""{model.ClassName}""));
            builder.Services.AddKeyed{Helpers.GetLifeTimeSyntax(model.Lifetime)}<{model.ClassName}, {model.ClassName}>(""{model.ClassName}"", {Helpers.ToSnakeCase(model.ClassName)}Factory);
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

{Helpers.GenerateConstructorParameterInitializationSyntax(model.Constructor,model.OptionType)}

            return new {model.ClassName}({Helpers.GenerateConstructorSyntax(model.Constructor,model.OptionType)});
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

                _lastAction = new KeyValuePair<string, string>($"{Helpers.ToSnakeCase(model.ClassName)}_BuilderExtensions.g.cs", builderExtensionSyntax);
                _lastModel = model;
            }
            context.AddSource(_lastAction.Key, _lastAction.Value);
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

        internal static void GenerateKeylessComponentOptionSyntax(SourceProductionContext context, ComponentModel model)
        {
            Helpers.GenerateOptionSyntax(context,model.Constructor,model.OptionType);
        }
    }
}