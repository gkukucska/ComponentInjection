using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Injectables
{
    internal class KeyedServiceModel : ServiceModel
    {

        public string ServiceKey { get; }

        public KeyedServiceModel(string className, List<string> implementationCollection, string lifetime, string serviceKey) : base(className, implementationCollection, lifetime)
        {
            ServiceKey = serviceKey;
        }
    }
}