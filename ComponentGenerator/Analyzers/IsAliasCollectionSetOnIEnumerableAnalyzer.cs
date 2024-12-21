﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ComponentGenerator.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class IsAliasCollectionSetOnIEnumerableAnalyzer: DiagnosticAnalyzer
    {
        internal const string DiagnosticId = "COMP001";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
                id: DiagnosticId,
                title: "AliasCollection can only be attached to IEnumerable",
                messageFormat: "AliasCollection cannot be built from Type '{0}'.",
                category: "Design",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                description: "The AliasCollection attribute needs to be attached to IEnumerable."
                );
        
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.ConstructorDeclaration);
        }

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is ConstructorDeclarationSyntax constructorDeclarationSyntax))
                return;

            var parameters = constructorDeclarationSyntax.ParameterList.Parameters;

            var aliasCollectionParameters = parameters.Where(parameter=>GetAttributes(parameter).Any(IsAliasCollection));

            foreach (var parameter in aliasCollectionParameters)
            {
                if (!(parameter.Type is GenericNameSyntax genericNameSyntax) || !parameter.Type.ToString().StartsWith("IEnumerable"))
                {
                    var attributeLocation = parameter.GetLocation();
                    var diagnostic = Diagnostic.Create(Rule, attributeLocation, parameter.Type.ToString());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
        
        private static IEnumerable<AttributeSyntax> GetAttributes(ParameterSyntax parameterSyntax)
        => parameterSyntax.AttributeLists.SelectMany(y => y.Attributes);

        private static bool IsAliasCollection(AttributeSyntax syntax)
        => syntax.Name.ToString().StartsWith("AliasCollection");
    }
}