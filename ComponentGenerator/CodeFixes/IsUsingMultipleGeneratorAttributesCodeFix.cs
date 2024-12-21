using ComponentGenerator.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ComponentGenerator.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(IsUsingMultipleGeneratorAttributesCodeFix)), Shared]
    public class IsUsingMultipleGeneratorAttributesCodeFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(IsUsingMultipleGeneratorAttributesAnalyzer.DiagnosticId);

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
            foreach (var diagnostic in context.Diagnostics)
            {
                var location = diagnostic.Location.SourceSpan;
                var declaration = root.FindToken(location.Start).Parent.AncestorsAndSelf()
                    .OfType<ClassDeclarationSyntax>().First();
                var attribute = root.FindToken(location.Start).Parent.Parent as AttributeSyntax;
                context.RegisterCodeFix(
                    CodeAction.Create($"Remove {attribute.Name.ToString()} attribute",
                        token => RemoveAttribute(context.Document, declaration, attribute, token)), diagnostic);
            }
        }

        private async Task<Document> RemoveAttribute(Document document, ClassDeclarationSyntax declaration,
            AttributeSyntax node, CancellationToken token)
        {
            // Replace the old local declaration with the new local declaration.
            var oldRoot = await document.GetSyntaxRootAsync(token);
            var oldAttributes = declaration.AttributeLists.ToList();
            var newAttributes = new SyntaxList<AttributeListSyntax>();
            foreach (var attributeListSyntax in oldAttributes)
            {
                if (attributeListSyntax.Attributes.All(attribute => attribute.Name.ToString() != node.Name.ToString()))
                {
                    newAttributes = newAttributes.Add(attributeListSyntax);
                    continue;
                }

                var newAttributeSyntaxes = attributeListSyntax.Attributes.Remove(attributeListSyntax.Attributes.First(attribute => attribute.Name.ToString() == node.Name.ToString()));
                foreach (var newAttribute in newAttributeSyntaxes)
                {
                    newAttributes= newAttributes.Add(AttributeList(SingletonSeparatedList<AttributeSyntax>(newAttribute)));
                }
            }

            var newRoot = oldRoot.ReplaceNode(declaration, declaration.WithAttributeLists(newAttributes));

            // Return document with transformed tree.
            return document.WithSyntaxRoot(newRoot);
        }
    }
}