using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComponentGenerator.ComponentBuilder
{
    [Generator]
    public class ComponentBuilderGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(HelperSyntaxGenerators.GenerateAttributes);
            var componentProvider = context.SyntaxProvider.ForAttributeWithMetadataName("ComponentGenerator.ComponentAttribute", 
                                                                                        predicate: (node, _) => node is ClassDeclarationSyntax,
                                                                                        transform: ModelGenerators.GenerateModel);
            context.RegisterSourceOutput(componentProvider, ComponentBuilderGeneratorHelpers.GenerateComponentBuilderSyntax);
            context.RegisterSourceOutput(componentProvider, ComponentBuilderGeneratorHelpers.GenerateComponentOptionSyntax);
        }
    }
}
