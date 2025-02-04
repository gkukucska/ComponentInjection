using ComponentGenerator.Common.Models;
using ComponentGenerator.Common.Models.Injectables;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace ComponentGenerator.KeylessComponentBuilder
{
    internal static class ModelGenerators
    {

        internal static KeylessComponentModel GenerateModel(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            if (!(context.TargetSymbol is INamedTypeSymbol classSymbol))
            {
                return null;
            }

            var componentAttributeSymbol = context.Attributes.FirstOrDefault(x=>x.AttributeClass.Name == "KeylessComponentAttribute");
            
            if (componentAttributeSymbol is null)
                return null;

            var optionType = componentAttributeSymbol.ConstructorArguments[0].Value.ToString();
            var lifetime = componentAttributeSymbol.ConstructorArguments[1].Value.ToString();
            var interfaceCollection = componentAttributeSymbol.ConstructorArguments[2].Values.Select(x=>x.Value.ToString()).ToList();

            var constructorParameters = ComponentBuilder.ModelGenerators.GetConstructorParameters(context.SemanticModel,classSymbol).ToList();

            var constructorModel = new ConstructorModel(constructorParameters);



            return new KeylessComponentModel(classSymbol.ToString(),
                constructorModel,
                interfaceCollection,
                lifetime,
                optionType
                );

        }
    }
}