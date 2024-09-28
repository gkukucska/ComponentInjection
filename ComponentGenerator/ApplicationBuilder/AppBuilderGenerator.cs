using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComponentGenerator.ApplicationBuilder
{
    [Generator]
    internal class AppBuilderGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(HelpersSyntaxGenerator.GenerateAttributes);

            var provider = context.SyntaxProvider.ForAttributeWithMetadataName("ComponentGenerator.ApplicationAttribute",
                                                                                predicate: (node, cts) => node is ClassDeclarationSyntax,
                                                                                transform: ModelGenrators.GenerateModel);
            context.RegisterSourceOutput(provider, AppBuilderGeneratorHelpers.GenerateApplicationBuilderSyntax);
        }
    }
}
