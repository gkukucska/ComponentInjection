using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace ComponentGenerator.ApplicationBuilder.Models
{
    internal class ApplicationModel
    {
        public string ComponentSection { get; }
        public List<string> ReferencedComponents { get; }
        public List<string> ReferencedServices { get; }
        public INamespaceSymbol ApplicationNamespace { get; }

        public ApplicationModel(string componentSection, List<string> referencedServices, List<string> referencedComponents, INamespaceSymbol applicationNamespace)
        {
            ComponentSection = componentSection;
            ReferencedServices = referencedServices;
            ReferencedComponents = referencedComponents;
            ApplicationNamespace = applicationNamespace;
        }
    }
}