using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace ComponentGenerator.ApplicationBuilder.Models
{
    internal class ApplicationModel
    {
        public string ComponentSection { get; }
        public List<string> ReferencedComponents { get; }
        public List<string> ReferencedKeylessComponents { get; }
        public List<string> HostedServices { get; }
        public List<string> ReferencedServices { get; }
        public List<string> ReferencedKeyedServices { get; }
        public INamespaceSymbol ApplicationNamespace { get; }

        public ApplicationModel(string componentSection, List<string> referencedServices, List<string> referencedKeyedServices, List<string> referencedComponents, List<string> referencedKeylessComponents, List<string> hostedServices, INamespaceSymbol applicationNamespace)
        {
            ComponentSection = componentSection;
            ReferencedServices = referencedServices;
            ReferencedKeyedServices = referencedKeyedServices;
            ReferencedComponents = referencedComponents;
            ReferencedKeylessComponents = referencedKeylessComponents;
            HostedServices = hostedServices;
            ApplicationNamespace = applicationNamespace;
        }
    }
}