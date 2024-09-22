using System.Collections.Generic;

namespace ComponentGenerator.ComponentBuilder.Models.Injectables
{
    internal class ServiceModel : InjectableModelBase
    {
        public ServiceModel(string className, ConstructorModel constructor, List<string> implementationCollection, string lifetime) : base(className, constructor, implementationCollection, lifetime)
        {
        }
    }
}