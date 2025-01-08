using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ComponentGenerator.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoesClassWithAliasHaveInjectionLogicAnalyzer:DiagnosticAnalyzer
    {
        
        internal const string DiagnosticId = "COMP004";

        private static readonly DiagnosticDescriptor _rule = new DiagnosticDescriptor(
                id: DiagnosticId,
                title: "Class with at least one aliased parameter should have component injection logic",
                messageFormat: "Class should have a component injection attribute to use aliases",
                category: "Design",
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: "Class with at least one aliased parameter should have component injection logic."
                );
        
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            if (!(context.Symbol is INamedTypeSymbol classDeclarationSymbol))
                return;

            var constructorSymbolCollection = classDeclarationSymbol.Constructors;
            var hasAnyConstructorParameterHaveAnAliasAttribute = constructorSymbolCollection.Any(constructor=>constructor.Parameters.Any(parameter => parameter.GetAttributes().Any(attribute => attribute.AttributeClass.ToString() == "ComponentGenerator.AliasAttribute" || attribute.AttributeClass.ToString() == "ComponentGenerator.AliasCollectionAttribute")));
            var hasClassComponentInjectionLogic = classDeclarationSymbol.GetAttributes().Any(attribute => attribute.AttributeClass.ToString() == "ComponentGenerator.ComponentAttribute" || attribute.AttributeClass.ToString() == "ComponentGenerator.KeylessComponentAttribute" || attribute.AttributeClass.ToString() == "ComponentGenerator.HostedServiceAttribute");

            if (hasAnyConstructorParameterHaveAnAliasAttribute && !hasClassComponentInjectionLogic)
            {
                foreach (var location in classDeclarationSymbol.Locations)
                {
                    var diagnostic = Diagnostic.Create(_rule, location);
                    context.ReportDiagnostic(diagnostic);
                }
                
            }
        }

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is ClassDeclarationSyntax classDeclarationSyntax))
                return;

            var parameters = classDeclarationSyntax.ParameterList.Parameters;

            var aliasCollectionParameters = parameters.Where(parameter=>GetAttributes(parameter).Any(IsAliasCollection));

            foreach (var parameter in aliasCollectionParameters)
            {
                if (!GetGenericNameSyntax(parameter)?.ToString().StartsWith("IEnumerable") ?? true)
                {
                    var attributeLocation = parameter.GetLocation();
                    var diagnostic = Diagnostic.Create(_rule, attributeLocation, parameter.Type?.ToString());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        internal static GenericNameSyntax GetGenericNameSyntax(ParameterSyntax node)
        {
            if (node.Type is NullableTypeSyntax nullableType)
            {
                return nullableType.ElementType as GenericNameSyntax;
            }
            return node.Type as GenericNameSyntax;
        }
        
        private static IEnumerable<AttributeSyntax> GetAttributes(ParameterSyntax parameterSyntax)
        => parameterSyntax.AttributeLists.SelectMany(y => y.Attributes);

        private static bool IsAliasCollection(AttributeSyntax syntax)
        => syntax.Name.ToString().StartsWith("AliasCollection");
    }
}