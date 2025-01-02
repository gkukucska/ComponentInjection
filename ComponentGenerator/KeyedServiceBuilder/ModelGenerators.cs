using ComponentGenerator.Common.Models.Injectables;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace ComponentGenerator.KeyedServiceBuilder
{
    internal static class ModelGenerators
    {
        internal static KeyedServiceModel GenerateModel(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            if (!(context.TargetSymbol is INamedTypeSymbol classSymbol))
            {
                return null;
            }

            var serviceAttributeSymbol = context.Attributes.FirstOrDefault(x=>x.AttributeClass.Name == "KeyedServiceAttribute");
            
            if (serviceAttributeSymbol is null)
                return null;

            var serviceKey = serviceAttributeSymbol.ConstructorArguments[0].Value.ToString();
            var lifetime = serviceAttributeSymbol.ConstructorArguments[1].Value.ToString();
            var interfaceCollection = serviceAttributeSymbol.ConstructorArguments[2].Values.Select(x => x.Value.ToString()).ToList();

            var className = classSymbol.ToString();
            return new KeyedServiceModel(className, interfaceCollection, lifetime, serviceKey);
        }
    }
}
