using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComponentGenerator.ServiceBuilder
{
    [Generator]
    internal class ServiceBuilderGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(HelperSyntaxGenerators.GenerateAttributes);
            var componentProvider = context.SyntaxProvider.ForAttributeWithMetadataName("ComponentGenerator.ServiceAttribute",
                                                                                        predicate: (node, _) => node is ClassDeclarationSyntax,
                                                                                        transform: ModelGenerators.GenerateModel);
            context.RegisterSourceOutput(componentProvider, ServiceGeneratorBuilderHelpers.GenerateServiceBuilderSyntax);
        }
    }
}
