using ComponentGenerator.Common.Models.Injectables;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace ComponentGenerator.ServiceBuilder
{
    internal static class ServiceGeneratorBuilderHelpers
    {
        internal static void GenerateServiceBuilderSyntax(SourceProductionContext context, ServiceModel model)
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
        public static IHostApplicationBuilder InstallAsService_{Helpers.ToSnakeCase(model.ClassName)}(this IHostApplicationBuilder builder)
        {{
            builder.Services.Add{GetLifeTimeSyntax(model.Lifetime)}<{model.ClassName}, {model.ClassName}>();
{GenerateProxyFactoryRegistrationSyntax(model)}
            return builder;
        }}

        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        private static {model.ClassName} {Helpers.ToSnakeCase(model.ClassName)}ProxyFactory(IServiceProvider provider)
        {{
            return provider.GetRequiredService<{model.ClassName}>();
        }}
    }}
}}
            ";
            context.AddSource($"{Helpers.ToSnakeCase(model.ClassName)}_BuilderExtensions.g.cs", builderExtensionSyntax);
        }
        private static string GenerateProxyFactoryRegistrationSyntax(ServiceModel model)
        {
            var builder = new StringBuilder();
            foreach (var implementation in model.ImplementationCollection)
            {
                builder.AppendLine($@"              builder.Services.Add{GetLifeTimeSyntax(model.Lifetime)}<{implementation}, {model.ClassName}>({Helpers.ToSnakeCase(model.ClassName)}ProxyFactory);");
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
