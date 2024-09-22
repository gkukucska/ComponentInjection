﻿using ComponentGenerator.ApplicationBuilder.Model;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ComponentGenerator.ApplicationBuilder
{
    internal static class ModelGenrators
    {

        internal static ApplicationModel GenerateModel(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            if (!(context.TargetSymbol is INamedTypeSymbol classSymbol))
            {
                return null;
            }
            var applicationNamespace = classSymbol.ContainingNamespace;
            var componentSection = context.Attributes.First().ConstructorArguments[0].Value.ToString();
            var types = context.SemanticModel.Compilation.SourceModule.ReferencedAssemblySymbols.SelectMany(a =>
            {
                try
                {
                    if (a.Identity.Name.StartsWith("mscorlib") ||
                        a.Identity.Name.StartsWith("System") ||
                        a.Identity.Name.StartsWith("Microsoft") ||
                        a.Identity.Name.StartsWith("Windows"))
                    {
                        return Enumerable.Empty<ITypeSymbol>();
                    }

                    var main = a.Identity.Name.Split('.').Aggregate(a.GlobalNamespace, (s, c) => s.GetNamespaceMembers().Single(m => m.Name.Equals(c)));

                    return GetAllTypes(main);
                }
                catch
                {
                    return Enumerable.Empty<ITypeSymbol>();
                }
            });

            var componentSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName("ComponentGenerator.ComponentAttribute");

            var components = types.Where(x => x.GetAttributes().Any(a => a.AttributeClass.Name.Equals(componentSymbol.Name))).OfType<INamedTypeSymbol>().Select(x => x.ToString()).ToList();

            return new ApplicationModel
            {
                ApplicationNamespace = applicationNamespace,
                ComponentSection = componentSection,
                ReferencedComponents = components
            };
        }

        private static IEnumerable<ITypeSymbol> GetAllTypes(INamespaceSymbol root)
        {
            foreach (var namespaceOrTypeSymbol in root.GetMembers())
            {
                if (namespaceOrTypeSymbol is INamespaceSymbol @namespace) foreach (var nested in GetAllTypes(@namespace)) yield return nested;

                else if (namespaceOrTypeSymbol is ITypeSymbol type) yield return type;
            }
        }
    }
}