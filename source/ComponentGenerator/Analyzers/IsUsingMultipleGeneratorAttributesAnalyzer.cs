using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace ComponentGenerator.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IsUsingMultipleGeneratorAttributesAnalyzer : DiagnosticAnalyzer
    {
        internal const string DiagnosticId = "COMP003";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: DiagnosticId,
            title: "Cannot use more than one component generator attribute",
            messageFormat: "Maximum one component generator attribute is allowed",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Maximum one component generator attribute is allowed."
        );
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
        
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            if(!(context.ContainingSymbol is INamedTypeSymbol classSymbol))
               return;
            
            var attributes = classSymbol.GetAttributes();
            var componentAttributes = attributes.Where(x => x.AttributeClass.BaseType.Name=="ComponentAttributeBase").ToList();
            if (componentAttributes.Count>1)
            {
                foreach (var attribute in componentAttributes)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, attribute.ApplicationSyntaxReference.GetSyntax().GetLocation()));
                }
            }
        }
    }
}