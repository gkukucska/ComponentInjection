using ComponentGenerator.Common.Models.Injectables;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ComponentGenerator.KeyedServiceBuilder
{
    internal static class KeyedServiceGeneratorBuilderHelpers
    {
        private static KeyedServiceModel _lastModel;
        private static KeyValuePair<string, string> _lastAction;

        internal static void GenerateKeyedServiceBuilderSyntax(SourceProductionContext context, KeyedServiceModel model)
        {
            if (model is null)
            {
                return;
            }

            if (_lastModel != model)
            {
                var builderExtensionSyntax = $@"//compiler generated
#nullable disable
using System.CodeDom.Compiler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        public static IHostApplicationBuilder InstallAsKeyedService_{Helpers.ToSnakeCase(model.ClassName)}(this IHostApplicationBuilder builder)
        {{
            builder.Services.AddKeyed{Helpers.GetLifeTimeSyntax(model.Lifetime)}<{model.ClassName}, {model.ClassName}>(""{model.ServiceKey}"");
{GenerateProxyFactoryRegistrationSyntax(model)}
            return builder;
        }}

        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        private static {model.ClassName} {Helpers.ToSnakeCase(model.ClassName)}ProxyFactory(IServiceProvider provider, object key)
        {{
            return provider.GetRequiredKeyedService<{model.ClassName}>(key);
        }}
    }}
}}
            ";

                _lastAction = new KeyValuePair<string, string>($"{Helpers.ToSnakeCase(model.ClassName)}_BuilderExtensions.g.cs", builderExtensionSyntax);
                _lastModel = model;
            }
            context.AddSource(_lastAction.Key, _lastAction.Value);
        }
        private static string GenerateProxyFactoryRegistrationSyntax(KeyedServiceModel model)
        {
            var builder = new StringBuilder();
            foreach (var implementation in model.ImplementationCollection)
            {
                builder.AppendLine($@"              builder.Services.AddKeyed{Helpers.GetLifeTimeSyntax(model.Lifetime)}<{implementation}, {model.ClassName}>({Helpers.ToSnakeCase(model.ClassName)}ProxyFactory);");
            }
            return builder.ToString();
        }
    }
}
