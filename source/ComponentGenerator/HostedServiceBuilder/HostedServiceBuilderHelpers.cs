using ComponentGenerator.Common.Models;
using ComponentGenerator.Common.Models.Injectables;
using ComponentGenerator.Common.Models.Parameters;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ComponentGenerator.HostedServiceBuilder
{
    internal static class HostedServiceBuilderHelpers
    {
        private static HostedServiceModel _lastModel;
        private static KeyValuePair<string, string> _lastModelAction;

        internal static void GenerateHostedServiceBuilderSyntax(SourceProductionContext context,
            HostedServiceModel model)
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
        public static IHostApplicationBuilder InstallAsHostedService_{Helpers.ToSnakeCase(model.ClassName)}(this IHostApplicationBuilder builder)
        {{
            builder.Services.AddOptions<{model.OptionType}>(""{model.ClassName}"").Bind(builder.Configuration.GetSection(""{model.ClassName}""));
            builder.Services.AddHostedService<{model.ClassName}>({Helpers.ToSnakeCase(model.ClassName)}Factory);
            return builder;
        }}

        [CompilerGenerated]
        [ExcludeFromCodeCoverage]
        [GeneratedCode(""{Assembly.GetExecutingAssembly().GetName().Name}"", ""{Assembly.GetExecutingAssembly().GetName().Version}"")]
        private static {model.ClassName} {Helpers.ToSnakeCase(model.ClassName)}Factory(IServiceProvider provider)
        {{
            var snapshot = provider.GetRequiredService<IOptionsSnapshot<{model.OptionType}>>();
            var options = snapshot.Get(""{model.ClassName}"");

{Helpers.GenerateConstructorParameterInitializationSyntax(model.Constructor,model.OptionType)}

            return new {model.ClassName}({Helpers.GenerateConstructorSyntax(model.Constructor, model.OptionType)});
        }}
    }}
}}
            ";

                _lastModelAction = new KeyValuePair<string, string>($"{Helpers.ToSnakeCase(model.ClassName)}_BuilderExtensions.g.cs", builderExtensionSyntax);
                _lastModel = model;
            }
            context.AddSource(_lastModelAction.Key, _lastModelAction.Value);
        }

        public static void GenerateHostedServiceOptionSyntax(SourceProductionContext context, HostedServiceModel model)
        {
            Helpers.GenerateOptionSyntax(context,model.Constructor,model.OptionType);
        }
    }
}