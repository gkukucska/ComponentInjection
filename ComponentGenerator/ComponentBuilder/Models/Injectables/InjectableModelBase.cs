using System.Collections.Generic;

namespace ComponentGenerator.ComponentBuilder.Models.Injectables
{
    internal class InjectableModelBase
    {
        public string NameSpace { get; }

        public string ClassName { get; }
        public ConstructorModel Constructor { get; }
        public List<string> ImplementationCollection { get; }
        public string Lifetime { get; }

        public InjectableModelBase(string nameSpace, string className, ConstructorModel constructor, List<string> implementationCollection, string lifetime)
        {
            NameSpace = nameSpace;
            ClassName = className;
            Constructor = constructor;
            ImplementationCollection = implementationCollection;
            Lifetime = lifetime;
        }
    }
}