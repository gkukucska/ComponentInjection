using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComponentGenerator.KeylessComponentBuilder
{
    [Generator]
    public class KeylessComponentBuilderGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(HelperSyntaxGenerators.GenerateAttributes);
            var componentProvider = context.SyntaxProvider.ForAttributeWithMetadataName("ComponentGenerator.KeylessComponentAttribute", 
                                                                                        predicate: (node, _) => node is ClassDeclarationSyntax,
                                                                                        transform: ModelGenerators.GenerateModel);
            context.RegisterSourceOutput(componentProvider, KeylessComponentBuilderGeneratorHelpers.GenerateKeylessComponentBuilderSyntax);
            context.RegisterSourceOutput(componentProvider, KeylessComponentBuilderGeneratorHelpers.GenerateKeylessComponentOptionSyntax);
        }
    }
}
