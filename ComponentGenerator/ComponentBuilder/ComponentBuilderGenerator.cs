using ComponentGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace ComponentGenerator.ComponentBuilder
{
    [Generator]
    public class ComponentBuilderGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(HelperSyntaxGenerators.GenerateAttributes);
            var provider = context.SyntaxProvider.CreateSyntaxProvider(predicate: (node, _) => node is ClassDeclarationSyntax,
                                                                       transform: ModelGenerators.GenerateModel);
            context.RegisterSourceOutput(provider, ComponentBuilderGeneratorHelpers.GenerateComponentBuilderSyntax);
            context.RegisterSourceOutput(provider, ComponentBuilderGeneratorHelpers.GenerateComponentOptionSyntax);
        }
    }
}
