using ComponentGenerator.Common.Models;
using ComponentGenerator.Common.Models.Injectables;
using ComponentGenerator.Common.Models.Parameters;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ComponentGenerator.ComponentBuilder
{
    internal static class ModelGenerators
    {

        internal static ComponentModel GenerateModel(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            if (!(context.TargetSymbol is INamedTypeSymbol classSymbol))
            {
                return null;
            }

            var componentAttributeSymbol = context.Attributes.First();

            var optionType = componentAttributeSymbol.ConstructorArguments[0].Value.ToString();
            var lifetime = componentAttributeSymbol.ConstructorArguments[1].Value.ToString();
            var interfaceCollection = componentAttributeSymbol.ConstructorArguments[2].Values.Select(x=>x.Value.ToString()).ToList();

            var constructorParameters = GetConstructorParameters(context.SemanticModel,classSymbol).ToList();

            var constructorModel = new ConstructorModel(constructorParameters);



            return new ComponentModel(classSymbol.ToString(),
                constructorModel,
                interfaceCollection,
                lifetime,
                optionType
                );

        }

        private static IEnumerable<ParameterModelBase> GetConstructorParameters(SemanticModel semanticModel,INamedTypeSymbol classSymbol)
        {
            var constructorSymbol = classSymbol.Constructors.OrderByDescending(x => x.Parameters.Count()).FirstOrDefault();
            var aliasSymbol = semanticModel.Compilation.GetTypeByMetadataName("ComponentGenerator.AliasAttribute");
            var serviceKeySymbol = semanticModel.Compilation.GetTypeByMetadataName("Microsoft.Extensions.DependencyInjection.ServiceKeyAttribute");
            var keyedServiceSymbol = semanticModel.Compilation.GetTypeByMetadataName("Microsoft.Extensions.DependencyInjection.FromKeyedServicesAttribute");

            foreach (var parameterSymbol in constructorSymbol.Parameters)
            {
                var attributes = parameterSymbol.GetAttributes();
                if (attributes.Any(x => x.AttributeClass.Equals(aliasSymbol, SymbolEqualityComparer.Default)))
                {
                    yield return new AliasParameterModel(parameterSymbol.Name, parameterSymbol.Type.ToString());
                    continue;
                }
                if (attributes.Any(x => x.AttributeClass.Equals(serviceKeySymbol, SymbolEqualityComparer.Default)))
                {
                    yield return new ServiceKeyParameterModel(parameterSymbol.Name, parameterSymbol.Type.ToString());
                    continue;
                }
                var keyedServiceAttribute = attributes.FirstOrDefault(x => x.AttributeClass.Equals(keyedServiceSymbol, SymbolEqualityComparer.Default));
                if (!(keyedServiceAttribute is null))
                {
                    yield return new KeyedServiceParameterModel(parameterSymbol.Name, parameterSymbol.Type.ToString(),keyedServiceAttribute.ConstructorArguments.First().Value.ToString());
                    continue;
                }
                yield return new ServiceParameterModel
                (
                    parameterSymbol.Name,
                    parameterSymbol.Type.ToString()
                );
            }
        }
    }
}