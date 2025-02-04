using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace ComponentGenerator.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class HasClassActivatorUtilitiesConstructorAttributeAnalyzer : DiagnosticAnalyzer
    {
        internal const string DiagnosticId = "COMP002";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: DiagnosticId,
            title: "ActivatorUtilitiesConstructorAttribute is ignored",
            messageFormat: "ActivatorUtilitiesConstructorAttribute will be ignored in source generated component injection",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "ActivatorUtilitiesConstructorAttribute is ignored."
        );
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
        
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.ConstructorDeclaration);
        }

        private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is ConstructorDeclarationSyntax constructorDeclarationSyntax))
                return;
            
            var constructorAttributes = constructorDeclarationSyntax.AttributeLists.SelectMany(list => list.Attributes).ToList();
            
            if(constructorAttributes.All(attribute => attribute.Name.ToString() != "ActivatorUtilitiesConstructor"))
                return;

            var activatorAttribute = constructorAttributes.First(attribute => attribute.Name.ToString() == "ActivatorUtilitiesConstructor");

            var attributeNames = context.ContainingSymbol.ContainingSymbol.GetAttributes().Select(attribute => attribute.AttributeClass.Name).ToList();
            if (attributeNames.Contains("KeylessComponentAttribute") || attributeNames.Contains("ComponentAttribute") || attributeNames.Contains("ServiceAttribute") || attributeNames.Contains("KeyedServiceAttribute"))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, activatorAttribute.GetLocation()));
            }
        }
    }
}