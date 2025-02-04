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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(IsAliasCollectionSetOnIEnumerableCodeFix)), Shared]
    internal class IsAliasCollectionSetOnIEnumerableCodeFix:CodeFixProvider
    {

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(IsAliasCollectionSetOnIEnumerableAnalyzer.DiagnosticId);
        
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
            foreach (var diagnostic in context.Diagnostics)
            {
                var location = diagnostic.Location.SourceSpan;            
                var declaration = root.FindToken(location.Start).Parent.AncestorsAndSelf().OfType<ParameterSyntax>().First();
                var genericNameSyntax = IsAliasCollectionSetOnIEnumerableAnalyzer.GetGenericNameSyntax(declaration);
                if (genericNameSyntax?.TypeArgumentList.Arguments.Count==1)
                {
                    context.RegisterCodeFix(CodeAction.Create("Convert parameter to IEnumerable",token => MakeIEnumerableAsync(context.Document,genericNameSyntax,token)), diagnostic);
                }
            }
        }

        private async Task<Document> MakeIEnumerableAsync(Document document, GenericNameSyntax genericNameSyntax, CancellationToken token)
        {
            
            // Replace the old local declaration with the new local declaration.
            var oldRoot = await document.GetSyntaxRootAsync(token);
            var newRoot = oldRoot.ReplaceNode(genericNameSyntax,GenericName(Identifier("IEnumerable")).WithTypeArgumentList(genericNameSyntax.TypeArgumentList));

            // Return document with transformed tree.
            return document.WithSyntaxRoot(newRoot);
        }
    }
}