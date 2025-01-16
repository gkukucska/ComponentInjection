using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComponentGenerator.HostedServiceBuilder
{
    [Generator]
    internal class HostedServiceValidatorGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var componentProvider = context.SyntaxProvider.ForAttributeWithMetadataName("ComponentGenerator.HostedServiceAttribute", 
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: ModelGenerators.GenerateModel);
            context.RegisterSourceOutput(componentProvider, HostedServiceValidatorGeneratorHelpers.GenerateComponentBuilderSyntax);
        }
    }
}