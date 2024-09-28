using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Injectables
{
    internal class InjectableModelBase
    {
        public string ClassName { get; }
        public List<string> ImplementationCollection { get; }
        public string Lifetime { get; }

        public InjectableModelBase(string className, List<string> implementationCollection, string lifetime)
        {
            ClassName = className;
            ImplementationCollection = implementationCollection;
            Lifetime = lifetime;
        }
    }
}