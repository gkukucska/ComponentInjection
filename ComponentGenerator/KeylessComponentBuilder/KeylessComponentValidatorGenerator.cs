using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComponentGenerator.KeylessComponentBuilder
{
    [Generator]
    internal class KeylessComponentValidatorGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var componentProvider = context.SyntaxProvider.ForAttributeWithMetadataName("ComponentGenerator.KeylessComponentAttribute", 
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: ModelGenerators.GenerateModel);
            context.RegisterSourceOutput(componentProvider, KeylessComponentValidatorGeneratorHelpers.GenerateComponentBuilderSyntax);
        }
    }
}