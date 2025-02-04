using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComponentGenerator.ApplicationBuilder
{
    [Generator]
    internal class AppValidatorGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var provider = context.SyntaxProvider.ForAttributeWithMetadataName("ComponentGenerator.ApplicationAttribute",
                predicate: (node, cts) => node is ClassDeclarationSyntax,
                transform: ModelGenrators.GenerateModel);
            context.RegisterSourceOutput(provider, AppValidatorGeneratorHelpers.GenerateApplicationValidatorSyntax);
        }
    }
}