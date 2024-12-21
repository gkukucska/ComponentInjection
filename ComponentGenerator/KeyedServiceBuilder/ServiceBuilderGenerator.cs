using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComponentGenerator.KeyedServiceBuilder
{
    [Generator]
    internal class ServiceBuilderGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(HelperSyntaxGenerators.GenerateAttributes);
            var componentProvider = context.SyntaxProvider.ForAttributeWithMetadataName("ComponentGenerator.KeyedServiceAttribute",
                                                                                        predicate: (node, _) => node is ClassDeclarationSyntax,
                                                                                        transform: ModelGenerators.GenerateModel);
            context.RegisterSourceOutput(componentProvider, KeyedServiceGeneratorBuilderHelpers.GenerateKeyedServiceBuilderSyntax);
        }
    }
}
