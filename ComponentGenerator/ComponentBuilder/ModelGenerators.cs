using ComponentGenerator.ComponentBuilder.Models;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace ComponentGenerator.ComponentBuilder
{
    internal static class ModelGenerators
    {

        internal static ComponentModel GenerateModel(GeneratorSyntaxContext context, CancellationToken token)
        {
            var componentSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName("ComponentGenerator.ComponentAttribute");

            var classSymbol = context.SemanticModel.GetDeclaredSymbol(context.Node, token) as INamedTypeSymbol;
            if (classSymbol?.GetAttributes().All(x => !x.AttributeClass.Equals(componentSymbol, SymbolEqualityComparer.Default)) != false)
            {
                return null;
            }

            var componentModel = new ComponentModel
            {
                Namespace = classSymbol.ContainingNamespace.ToString(),
                ClassName = classSymbol.Name,
            };

            var aliasSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName("ComponentGenerator.AliasAttribute");

            var componentAttributeSymbol = classSymbol.GetAttributes().First(x => x.AttributeClass.Equals(componentSymbol, SymbolEqualityComparer.Default));

            componentModel.InterfaceType = componentAttributeSymbol.ConstructorArguments[0].Value.ToString();
            componentModel.OptionType = componentAttributeSymbol.ConstructorArguments[1].Value.ToString();
            componentModel.Lifetime = componentAttributeSymbol.ConstructorArguments[2].Value.ToString();

            var constructorSymbol = classSymbol.Constructors.OrderByDescending(x => x.Parameters.Count()).FirstOrDefault();

            var constructorModel = new ConstructorModel();

            foreach (var parameterSymbol in constructorSymbol.Parameters)
            {
                constructorModel.Parameters.Add(new ParameterModel
                {
                    Name = parameterSymbol.Name,
                    Type = parameterSymbol.Type.ToString(),
                    IsAlias = parameterSymbol.GetAttributes().Any(x => x.AttributeClass.Equals(aliasSymbol, SymbolEqualityComparer.Default))
                });
            }

            componentModel.Constructor = constructorModel;
            return componentModel;

        }
    }
}