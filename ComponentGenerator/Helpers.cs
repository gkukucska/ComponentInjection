using ComponentGenerator.Common.Models;
using ComponentGenerator.Common.Models.Parameters;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ComponentGenerator
{
    internal static class Helpers
    {

        internal static string CapitalizeFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }
        internal static string ToSnakeCase(string source)
        {
            return source.Replace('.', '_');
        }

        internal static string GetLifeTimeSyntax(string lifetime)
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

        internal static string GenerateRequiredSyntaxIfNeeded(ParameterModelBase parameterModel)
        {
            return parameterModel.IsOptional ? string.Empty : "Required";
        }

        internal static string GenerateConstructorParameterInitializationSyntax(ConstructorModel model, string optionType)
        {
            var builder = new StringBuilder();
            foreach (var parameter in model.Parameters)
            {
                if (parameter is AliasParameterModel aliasParameterModel)
                {
                    builder.AppendLine($@"            var {aliasParameterModel.Name} = provider.Get{GenerateRequiredSyntaxIfNeeded(parameter)}KeyedService<{aliasParameterModel.Type}>(options.{Helpers.CapitalizeFirstLetter(aliasParameterModel.Name)});");
                    continue;
                }
                if (parameter is AliasCollectionParameterModel aliasCollectionParameterModel)
                {
                    builder.AppendLine($@"            var {aliasCollectionParameterModel.Name} = options.{Helpers.CapitalizeFirstLetter(aliasCollectionParameterModel.Name)}.Select(alias=>provider.Get{GenerateRequiredSyntaxIfNeeded(parameter)}KeyedService<{aliasCollectionParameterModel.Type}>(alias));");
                    continue;
                }
                if (parameter is KeyedServiceParameterModel keyedParameterModel)
                {
                    builder.AppendLine($@"            var {keyedParameterModel.Name} = provider.Get{GenerateRequiredSyntaxIfNeeded(parameter)}KeyedService<{keyedParameterModel.Type}>(""{keyedParameterModel.ServiceKey}"");");
                    continue;
                }
                if (parameter.Type == optionType)
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

        internal static string GenerateConstructorSyntax(ConstructorModel model, string optionType)
        {
            var parameterSyntaxCollection = new List<string>();
            foreach (var parameter in model.Parameters)
            {
                if (parameter is AliasParameterModel || parameter is AliasCollectionParameterModel)
                {
                    parameterSyntaxCollection.Add(parameter.Name);
                    continue;
                }
                if (parameter is ServiceKeyParameterModel)
                {
                    parameterSyntaxCollection.Add("key");
                }
                if (parameter.Type == optionType)
                {
                    parameterSyntaxCollection.Add($"options");
                    continue;
                }
                parameterSyntaxCollection.Add(parameter.Name);
            }
            return string.Join(", ", parameterSyntaxCollection);
        }

        internal static void GenerateOptionSyntax(SourceProductionContext context, ConstructorModel model, string optionType)
        {
            if (!model.Parameters.OfType<AliasParameterModel>().Any() && !model.Parameters.OfType<AliasCollectionParameterModel>().Any())
            {
                return;
            }


            var optionClassName = optionType.Split('.').Last();
            var optionNamespace = string.Join(".", optionType.Split('.').Take(optionType.Split('.').Count() - 1));
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
{GenerateAliasProperties(model)}
    }}
}}
";

            context.AddSource($"{optionClassName}.g.cs", optionSyntax);
        }

        internal static string GenerateAliasProperties(ConstructorModel model)
        {
            var builder = new StringBuilder();
            foreach (var parameter in model.Parameters.OfType<AliasParameterModel>())
            {
                builder.AppendLine($@"        public string {Helpers.CapitalizeFirstLetter(parameter.Name)} {{ get; set; }}");
            }
            foreach (var parameter in model.Parameters.OfType<AliasCollectionParameterModel>())
            {
                builder.AppendLine($@"        public IEnumerable<string> {Helpers.CapitalizeFirstLetter(parameter.Name)} {{ get; set; }}");
            }

            return builder.ToString();
        }
    }
}