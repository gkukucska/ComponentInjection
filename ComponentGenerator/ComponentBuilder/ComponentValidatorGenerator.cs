using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComponentGenerator.ComponentBuilder
{
    [Generator]
    internal class ComponentValidatorGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var componentProvider = context.SyntaxProvider.ForAttributeWithMetadataName("ComponentGenerator.ComponentAttribute", 
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: ModelGenerators.GenerateModel);
            context.RegisterSourceOutput(componentProvider, ComponentValidatorGeneratorHelpers.GenerateComponentBuilderSyntax);
        }
    }
}