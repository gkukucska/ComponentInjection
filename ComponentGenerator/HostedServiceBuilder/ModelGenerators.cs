using ComponentGenerator.Common.Models;
using ComponentGenerator.Common.Models.Injectables;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace ComponentGenerator.HostedServiceBuilder
{
    internal static class ModelGenerators
    {
        public static HostedServiceModel GenerateModel(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            if (!(context.TargetSymbol is INamedTypeSymbol classSymbol))
            {
                return null;
            }

            var componentAttributeSymbol = context.Attributes.FirstOrDefault(x=>x.AttributeClass.Name == "HostedServiceAttribute");
            
            if (componentAttributeSymbol is null)
                return null;
            
            var optionType = componentAttributeSymbol.ConstructorArguments[0].Value.ToString();
            
            var constructorParameters = ComponentBuilder.ModelGenerators.GetConstructorParameters(context.SemanticModel, classSymbol).ToList();

            var constructorModel = new ConstructorModel(constructorParameters);

            var className = classSymbol.ToString();
            
            return new HostedServiceModel(className,constructorModel,optionType);
        }
    }
}