using System.Collections.Generic;

namespace ComponentGenerator.ComponentBuilder.Models.Injectables
{
    internal class KeyedServiceModel : ServiceModel
    {

        public string ServiceKey { get; }

        public KeyedServiceModel(string className, ConstructorModel constructor, List<string> implementationCollection, string lifetime, string serviceKey) : base(className, constructor, implementationCollection, lifetime)
        {
            ServiceKey = serviceKey;
        }
    }
}