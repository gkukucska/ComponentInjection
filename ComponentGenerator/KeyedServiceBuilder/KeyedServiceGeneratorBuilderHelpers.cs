using ComponentGenerator.Common.Models.Injectables;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace ComponentGenerator.KeyedServiceBuilder
{
    internal static class KeyedServiceGeneratorBuilderHelpers
    {
        internal static void GenerateKeyedServiceBuilderSyntax(SourceProductionContext context, KeyedServiceModel model)
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
        public static IHostApplicationBuilder Install{Helpers.ToSnakeCase(model.ClassName)}(this IHostApplicationBuilder builder)
        {{
            builder.Services.AddKeyed{GetLifeTimeSyntax(model.Lifetime)}<{model.ClassName}, {model.ClassName}>({model.ServiceKey});
{GenerateProxyFactoryRegistrationSyntax(model)}
            return builder;
        }}

        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        private static {model.ClassName} {Helpers.ToSnakeCase(model.ClassName)}ProxyFactory(IServiceProvider provider, objet key)
        {{
            return provider.GetRequiredService<{model.ClassName}>(key);
        }}
    }}
}}
            ";
            context.AddSource($"{Helpers.ToSnakeCase(model.ClassName)}_BuilderExtensions.g.cs", builderExtensionSyntax);
        }
        private static string GenerateProxyFactoryRegistrationSyntax(KeyedServiceModel model)
        {
            var builder = new StringBuilder();
            foreach (var implementation in model.ImplementationCollection)
            {
                builder.AppendLine($@"              builder.Services.AddKeyed{GetLifeTimeSyntax(model.Lifetime)}<{implementation}, {model.ClassName}>({Helpers.ToSnakeCase(model.ClassName)}ProxyFactory,{model.ServiceKey});");
            }
            return builder.ToString();
        }

        private static string GetLifeTimeSyntax(string lifetime)
        {
            switch (lifetime)
            {
                case "0":
                    return "Singleton";
                case "1":
                    return "Transient";
                case "2":
                    return "Scoped";
                default:
                    return string.Empty;
            }
        }
    }
}
