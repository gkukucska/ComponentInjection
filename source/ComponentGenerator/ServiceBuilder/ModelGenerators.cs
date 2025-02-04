using ComponentGenerator.Common.Models.Injectables;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace ComponentGenerator.ServiceBuilder
{
    internal static class ModelGenerators
    {
        internal static ServiceModel GenerateModel(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            if (!(context.TargetSymbol is INamedTypeSymbol classSymbol))
            {
                return null;
            }

            var serviceAttributeSymbol = context.Attributes.FirstOrDefault(x=>x.AttributeClass.Name == "ServiceAttribute");
            
            if (serviceAttributeSymbol is null)
                return null;

            var lifetime = serviceAttributeSymbol.ConstructorArguments[0].Value.ToString();
            var interfaceCollection = serviceAttributeSymbol.ConstructorArguments[1].Values.Select(x => x.Value.ToString()).ToList();

            var className = classSymbol.ToString();
            return new ServiceModel(className, interfaceCollection, lifetime);
        }
    }
}
