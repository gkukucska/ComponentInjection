using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComponentGenerator.HostedServiceBuilder
{
    [Generator]
    internal class HostedServiceBuilderGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(HelperSyntaxGenerators.GenerateAttributes);
            var componentProvider = context.SyntaxProvider.ForAttributeWithMetadataName("ComponentGenerator.HostedServiceAttribute", 
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: ModelGenerators.GenerateModel);
            context.RegisterSourceOutput(componentProvider, HostedServiceBuilderHelpers.GenerateHostedServiceBuilderSyntax);
            context.RegisterSourceOutput(componentProvider, HostedServiceBuilderHelpers.GenerateHostedServiceOptionSyntax);
        }
    }
}